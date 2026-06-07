using Microsoft.Extensions.Logging;
using Shared.Notifications;

namespace Infrastructure.Notifications
{
    public sealed class LoggingNotificationService
        : INotificationService
    {
        private readonly ILogger<LoggingNotificationService> _logger;

        public LoggingNotificationService(
            ILogger<LoggingNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(
            string subject,
            string message,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Notification: {Subject} {Message}",
                subject,
                message);

            return Task.CompletedTask;
        }
    }
}