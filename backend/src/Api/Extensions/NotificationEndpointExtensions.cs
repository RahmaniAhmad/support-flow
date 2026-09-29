
using Api.Features.Notifications.Commands.MarkAllAsRead;
using Api.Features.Notifications.Commands.MarkAsRead;
using Api.Features.Notifications.Queries.GetNotifications;
using Api.Features.Notifications.Queries.GetUnreadCount;
using Api.Features.Notifications.SignalR;

namespace Api.Extensions;

public static class NotificationEndpointExtensions
{
    public static WebApplication MapNotificationEndpoints(
        this WebApplication app)
    {
        app.MapGetNotifications();
        app.MapGetUnreadCount();
        app.MapMarkAsRead();
        app.MapMarkAllAsRead();

        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }
}