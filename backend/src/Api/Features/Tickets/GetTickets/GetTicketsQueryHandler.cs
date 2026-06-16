using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler
    : IRequestHandler<GetTicketsQuery, List<GetTicketsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketsQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetTicketsResponse>> Handle(
        GetTicketsQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Tickets
            .Where(x => x.CompanyId == _currentUser.CompanyId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new GetTicketsResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}