using MediatR;
using Shared.DomainEvents;
using Shared.Notifications;

namespace Api.Notifications
{
    public sealed class TicketCreatedNotificationHandler
        : INotificationHandler<TicketCreatedDomainEvent>
    {
        private readonly INotificationService _notificationService;

        public TicketCreatedNotificationHandler(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(
            TicketCreatedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            await _notificationService.SendAsync(
                "Ticket Created",
                $"Ticket {notification.Subject} created.",
                cancellationToken);
        }
    }
}