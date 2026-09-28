namespace Api.Notifications;

public static class NotificationEndpointExtensions
{
    public static WebApplication MapNotificationEndpoints(
        this WebApplication app)
    {
        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }
}