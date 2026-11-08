using Api.Errors;
using Api.Exceptions;
using Api.Features.Tickets.Attachments;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Authentication;
using Shared.Domain.Companies;
using Shared.Domain.Tickets;
using System.Data;

namespace Api.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler
    : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly TicketAttachmentProcessor _attachmentProcessor;


    public CreateTicketCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser,
        TicketAttachmentProcessor attachmentProcessor)
    {
        _db = db;
        _currentUser = currentUser;
        _attachmentProcessor = attachmentProcessor;
    }


    public async Task<Guid> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
                   ?? throw new BadRequestException(
                       CommonErrors.CompanyContextRequiredMessage,
                       CommonErrors.CompanyContextRequiredCode);

        if (request.CategoryId is Guid categoryId)
        {
            var categoryIsAvailable = await _db.TicketCategories
                .AnyAsync(
                    x => x.Id == categoryId
                        && x.CompanyId == companyId
                        && x.IsActive,
                    cancellationToken);

            if (!categoryIsAvailable)
            {
                throw new BadRequestException(
                    "The selected ticket category is unavailable.",
                    "TICKET_CATEGORY_UNAVAILABLE");
            }
        }

        var attachments = await TicketAttachmentValidator.ValidateFilesAsync(
            request.Attachments,
            cancellationToken);

        var counter = await _db.CompanyTicketCounters
          .FirstOrDefaultAsync(
              x => x.CompanyId == companyId,
              cancellationToken);

        if (counter is null)
        {
            counter = CompanyTicketCounter.Create(companyId);

            _db.CompanyTicketCounters.Add(counter);
        }

        var ticketNumber = counter.GetNextNumber();


        var ticket = Ticket.Create(
            companyId,
            _currentUser.UserId,
            request.Subject,
            request.Description,
            request.CategoryId);


        ticket.AssignTicketNumber(ticketNumber);

        _db.Tickets.Add(ticket);

        if (attachments.Count == 0)
        {
            await _db.SaveChangesAsync(cancellationToken);
            return ticket.Id;
        }

        await using var transaction = _db.Database.IsRelational()
            ? await _db.Database.BeginTransactionAsync(
                IsolationLevel.ReadCommitted,
                cancellationToken)
            : null;
        IReadOnlyList<Attachment> stagedAttachments = [];

        try
        {
            stagedAttachments = await _attachmentProcessor.UploadAndStageAsync(
                ticket,
                _currentUser.UserId,
                attachments,
                cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);
            if (transaction is not null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            if (transaction is not null)
            {
                await transaction.RollbackAsync(CancellationToken.None);
            }

            await _attachmentProcessor.CleanupAsync(
                stagedAttachments.Select(x => x.StoragePath));
            throw;
        }

        return ticket.Id;
    }
}