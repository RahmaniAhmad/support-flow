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


    public GetUnreadCountQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task<GetUnreadCountResponse> Handle(
        GetUnreadCountQuery request,
        CancellationToken cancellationToken)
    {
        var count =
            await _db.Notifications
                .AsNoTracking()
                .CountAsync(
                    x =>
                        x.UserId == _currentUser.UserId &&
                        x.ReadAtUtc == null,
                    cancellationToken);


        return new GetUnreadCountResponse(
            count);
    }
}