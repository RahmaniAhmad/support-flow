using Shared.Domain.Notifications;

namespace Shared.Notifications;

public interface INotificationService
{
    Task SendAsync(
        Guid userId,
        Notification notification,
        CancellationToken cancellationToken);
}
