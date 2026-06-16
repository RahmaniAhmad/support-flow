using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetUnassignedTickets;

public sealed class GetUnassignedTicketsQueryHandler
    : IRequestHandler<GetUnassignedTicketsQuery, List<GetUnassignedTicketsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetUnassignedTicketsQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetUnassignedTicketsResponse>> Handle(
        GetUnassignedTicketsQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Tickets
            .Where(x =>
                x.CompanyId == _currentUser.CompanyId &&
                x.AssignedToUserId == null)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new GetUnassignedTicketsResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}