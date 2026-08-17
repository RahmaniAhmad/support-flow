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

    public GetUnassignedTicketsQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<UnassignedTicketResponse>> Handle(
        GetUnassignedTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Tickets
            .AsNoTracking()
            .Where(x => x.AssignedToUserId == null);

        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(request.Limit)
            .Select(x => new UnassignedTicketResponse(
                x.Id,
                x.TicketNumber,
                x.Subject,
                _db.Users
                    .Where(u => u.Id == x.CreatedByUserId)
                    .Select(u => u.FirstName + " " + u.LastName)
                    .First(),
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}