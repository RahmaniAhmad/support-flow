using Microsoft.AspNetCore.SignalR;
using Shared.Domain.Notifications;
using Shared.Notifications;

namespace Api.Notifications;

public sealed class SignalRNotificationService
    : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationService(
        IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAsync(
        Guid userId,
        Notification notification,
        CancellationToken cancellationToken)
    {
        await _hubContext
            .Clients
            .User(userId.ToString())
            .SendAsync(
                "notification",
                notification,
                cancellationToken);
    }
}