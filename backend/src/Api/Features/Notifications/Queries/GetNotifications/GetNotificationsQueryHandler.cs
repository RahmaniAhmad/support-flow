using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed class GetNotificationsQueryHandler
    : IRequestHandler<
        GetNotificationsQuery,
        List<GetNotificationsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public GetNotificationsQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task<List<GetNotificationsResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await _db.Notifications
                .AsNoTracking()
                .Where(x =>
                    x.UserId == _currentUser.UserId)
                .OrderByDescending(x =>
                    x.CreatedAtUtc)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x =>
                    new GetNotificationsResponse(
                        x.Id,
                        x.TicketId,
                        x.Type,
                        x.Title,
                        x.Message,
                        x.ReadAtUtc != null,
                        x.CreatedAtUtc))
                .ToListAsync(cancellationToken);


        return notifications;
    }
}