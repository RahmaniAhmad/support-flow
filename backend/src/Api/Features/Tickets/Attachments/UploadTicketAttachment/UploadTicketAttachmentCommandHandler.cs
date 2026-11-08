using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Shared.Authentication;
using System.Data;

namespace Api.Features.Tickets.Attachments.UploadTicketAttachment;

public sealed class UploadTicketAttachmentCommandHandler
    : IRequestHandler<UploadTicketAttachmentCommand, TicketAttachmentResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _ticketAccess;
    private readonly TicketAttachmentProcessor _attachmentProcessor;
    private readonly ILogger<UploadTicketAttachmentCommandHandler> _logger;

    public UploadTicketAttachmentCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser,
        ITicketAccessService ticketAccess,
        TicketAttachmentProcessor attachmentProcessor,
        ILogger<UploadTicketAttachmentCommandHandler> logger)
    {
        _db = db;
        _currentUser = currentUser;
        _ticketAccess = ticketAccess;
        _attachmentProcessor = attachmentProcessor;
        _logger = logger;
    }

    public async Task<TicketAttachmentResponse> Handle(
        UploadTicketAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets)
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                TicketErrors.NotFoundMessage,
                TicketErrors.NotFoundCode);
        }

        var files = await TicketAttachmentValidator.ValidateFilesAsync(
            [request.File],
            cancellationToken);

        IDbContextTransaction? transaction = null;

        if (_db.Database.IsRelational())
        {
            transaction = await _db.Database.BeginTransactionAsync(
                IsolationLevel.Serializable,
                cancellationToken);
        }

        await using (transaction)
        {
            var attachmentCount = await _db.Attachments
                .CountAsync(
                    x => x.CompanyId == ticket.CompanyId &&
                         x.TicketId == ticket.Id,
                    cancellationToken);

            if (attachmentCount >=
                TicketAttachmentValidator.MaximumAttachmentsPerTicket)
            {
                throw new ConflictException(
                    "This ticket has reached the attachment limit.",
                    TicketAttachmentErrors.LimitReached);
            }

            IReadOnlyList<Shared.Domain.Tickets.Attachment> stagedAttachments = [];

            try
            {
                stagedAttachments = await _attachmentProcessor.UploadAndStageAsync(
                    ticket,
                    _currentUser.UserId,
                    files,
                    cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                if (transaction is not null)
                {
                    await transaction.CommitAsync(cancellationToken);
                }

                var attachment = stagedAttachments[0];
                return new TicketAttachmentResponse(
                    attachment.Id,
                    attachment.FileName,
                    attachment.ContentType,
                    attachment.FileSize,
                    attachment.UploadedByUserId,
                    attachment.CreatedAtUtc);
            }
            catch (Exception exception)
            {
                await _attachmentProcessor.CleanupAsync(
                    stagedAttachments.Select(x => x.StoragePath));

                if (exception is PostgresException
                    {
                        SqlState: PostgresErrorCodes.SerializationFailure
                    })
                {
                    throw new ConflictException(
                        "This ticket has reached the attachment limit.",
                        TicketAttachmentErrors.LimitReached);
                }

                throw;
            }
        }
    }
}
