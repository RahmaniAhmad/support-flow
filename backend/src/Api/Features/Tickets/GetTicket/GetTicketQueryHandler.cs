using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetTicket;

public sealed class GetTicketQueryHandler
    : IRequestHandler<GetTicketQuery, GetTicketResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<GetTicketResponse?> Handle(
        GetTicketQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.Tickets
            .Where(x =>
                x.Id == request.TicketId &&
                x.CompanyId == _currentUser.CompanyId)
            .Select(x => new GetTicketResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId,
                x.CreatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);
    }
}