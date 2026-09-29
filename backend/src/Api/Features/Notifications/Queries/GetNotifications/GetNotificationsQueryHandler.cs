using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts;

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed class GetNotificationsQueryHandler
    : IRequestHandler<
        GetNotificationsQuery,
        PagedResult<GetNotificationsResponse>>
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


    public async Task<PagedResult<GetNotificationsResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Notifications
        .AsNoTracking()
        .Where(x =>
            x.UserId == _currentUser.UserId);

        var totalCount =
                await query.CountAsync(
                    cancellationToken);

        var items =
                await query
                    .OrderByDescending(x =>
                        x.CreatedAtUtc)
                    .Skip(
                        (request.Page - 1) * request.PageSize)
                    .Take(
                        request.PageSize)
                    .Select(x =>
                        new GetNotificationsResponse(
                            x.Id,
                            x.TicketId,
                            x.Type,
                            x.Title,
                            x.Message,
                            x.ReadAtUtc,
                            x.CreatedAtUtc))
                    .ToListAsync(
                        cancellationToken);


        return new PagedResult<GetNotificationsResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize);
    }
}