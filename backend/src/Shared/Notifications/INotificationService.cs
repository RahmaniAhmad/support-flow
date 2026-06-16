namespace Shared.Notifications;

public interface INotificationService
{
    Task SendAsync(
        string subject,
        string message,
        CancellationToken cancellationToken);
}
