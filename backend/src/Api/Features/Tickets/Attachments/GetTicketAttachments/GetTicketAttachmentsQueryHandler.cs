using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.Attachments.GetTicketAttachments;

public sealed class GetTicketAttachmentsQueryHandler
    : IRequestHandler<
        GetTicketAttachmentsQuery,
        IReadOnlyList<TicketAttachmentResponse>?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;

    public GetTicketAttachmentsQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }

    public async Task<IReadOnlyList<TicketAttachmentResponse>?> Handle(
        GetTicketAttachmentsQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
            .Where(x => x.Id == request.TicketId)
            .Select(x => new { x.Id, x.CompanyId })
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket is null)
        {
            return null;
        }

        return await _db.Attachments
            .AsNoTracking()
            .Where(x =>
                x.CompanyId == ticket.CompanyId &&
                x.TicketId == ticket.Id)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => new TicketAttachmentResponse(
                x.Id,
                x.FileName,
                x.ContentType,
                x.FileSize,
                x.UploadedByUserId,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
