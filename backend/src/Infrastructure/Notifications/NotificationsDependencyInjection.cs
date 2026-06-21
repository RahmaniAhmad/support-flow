using Microsoft.Extensions.DependencyInjection;
using Shared.Notifications;

namespace Infrastructure.Notifications;

public static class NotificationsDependencyInjection
{
    public static IServiceCollection AddNotifications(
        this IServiceCollection services)
    {
        services.AddScoped<INotificationService, LoggingNotificationService>();

        return services;
    }
}