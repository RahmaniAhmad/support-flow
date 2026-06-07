using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Events;
using Shared.Notifications;

namespace Api.Notifications
{
    public sealed class TicketCreatedNotificationHandler
        : INotificationHandler<TicketCreated>
    {
        private readonly INotificationService _notificationService;

        public TicketCreatedNotificationHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(
            TicketCreated notification,
            CancellationToken cancellationToken)
        {
            await _notificationService.SendAsync(
                "Ticket Created",
                $"Ticket {notification.Subject} created.",
                cancellationToken);
        }
    }
}