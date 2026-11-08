using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Storage;

namespace Api.Features.Tickets.Attachments.DownloadTicketAttachment;

public sealed class DownloadTicketAttachmentQueryHandler
    : IRequestHandler<
        DownloadTicketAttachmentQuery,
        DownloadTicketAttachmentResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;
    private readonly IAttachmentStorage _storage;

    public DownloadTicketAttachmentQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess,
        IAttachmentStorage storage)
    {
        _db = db;
        _ticketAccess = ticketAccess;
        _storage = storage;
    }

    public async Task<DownloadTicketAttachmentResponse?> Handle(
        DownloadTicketAttachmentQuery request,
        CancellationToken cancellationToken)
    {
        var accessibleTickets = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking());

        var attachment = await _db.Attachments
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                accessibleTickets
                    .Any(ticket =>
                        ticket.Id == x.TicketId &&
                        ticket.CompanyId == x.CompanyId))
            .Select(x => new
            {
                x.FileName,
                x.ContentType,
                x.StoragePath
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (attachment is null)
        {
            return null;
        }

        var content = await _storage.OpenReadAsync(
            attachment.StoragePath,
            cancellationToken);

        return new DownloadTicketAttachmentResponse(
            content,
            attachment.FileName,
            attachment.ContentType);
    }
}
