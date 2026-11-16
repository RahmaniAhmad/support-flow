using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Notifications.Queries.GetUnreadCount;

public sealed class GetUnreadCountQueryHandler
    : IRequestHandler<
        GetUnreadCountQuery,
        GetUnreadCountResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _ticketAccess;


    public GetUnreadCountQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _currentUser = currentUser;
        _ticketAccess = ticketAccess;
    }


    public async Task<GetUnreadCountResponse> Handle(
        GetUnreadCountQuery request,
        CancellationToken cancellationToken)
    {
        var accessibleTicketIds = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
            .Select(x => x.Id);

        var count =
            await _db.Notifications
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.UserId == _currentUser.UserId &&
                        x.ReadAtUtc == null &&
                        accessibleTicketIds.Contains(x.TicketId),
                    cancellationToken);


        return new GetUnreadCountResponse(
            count);
    }
}