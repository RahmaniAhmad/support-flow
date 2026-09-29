

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsRequest(
    int Page = 1,
    int PageSize = 20);