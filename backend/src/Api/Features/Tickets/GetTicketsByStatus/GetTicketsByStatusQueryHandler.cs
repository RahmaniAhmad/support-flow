using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetTicketsByStatus;

public sealed class GetTicketsByStatusQueryHandler
    : IRequestHandler<GetTicketsByStatusQuery, List<GetTicketsByStatusResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketsByStatusQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetTicketsByStatusResponse>> Handle(
        GetTicketsByStatusQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Tickets
            .Where(x =>
                x.CompanyId == _currentUser.CompanyId &&
                x.Status == request.Status)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new GetTicketsByStatusResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}