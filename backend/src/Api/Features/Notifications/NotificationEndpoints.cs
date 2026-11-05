
using Api.Features.Notifications.Commands.MarkAllAsRead;
using Api.Features.Notifications.Commands.MarkAsRead;
using Api.Features.Notifications.Queries.GetNotifications;
using Api.Features.Notifications.Queries.GetUnreadCount;
using Api.Features.Notifications.SignalR;

namespace Api.Features.Notifications;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotificationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetNotifications();
        endpoints.MapGetUnreadCount();
        endpoints.MapMarkAsRead();
        endpoints.MapMarkAllAsRead();

        endpoints.MapHub<NotificationHub>("/hubs/notifications");


        return endpoints;
    }
}
