using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Dashboard.Unassigned;

public sealed class GetUnassignedTicketsQueryHandler
    : IRequestHandler<
        GetUnassignedTicketsQuery,
        IReadOnlyList<UnassignedTicketResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;

    public GetUnassignedTicketsQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }

    public async Task<IReadOnlyList<UnassignedTicketResponse>> Handle(
        GetUnassignedTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
            .Where(x => x.AssignedToUserId == null);

        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(Math.Clamp(request.Limit, 1, 20))
            .Select(x => new UnassignedTicketResponse(
                x.Id,
                x.TicketNumber,
                x.Subject,
                _db.Users
                    .Where(u => u.Id == x.CreatedByUserId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .First(),
                x.Priority,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}