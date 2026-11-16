using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.Queries.GetTicketsByStatus;

public sealed class GetTicketsByStatusQueryHandler
    : IRequestHandler<GetTicketsByStatusQuery, List<GetTicketsByStatusResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;

    public GetTicketsByStatusQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }

    public async Task<List<GetTicketsByStatusResponse>> Handle(
        GetTicketsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        return await _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
            .Where(x =>
                x.Status == request.Status)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new GetTicketsByStatusResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.CategoryId,
                x.CategoryId != null
                    ? _db.TicketCategories
                        .Where(c => c.Id == x.CategoryId)
                        .Select(c => c.Name)
                        .FirstOrDefault()
                    : null,
                x.Status,
                x.Priority,
                x.AssignedToUserId,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}