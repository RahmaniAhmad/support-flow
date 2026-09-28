using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Notifications;
using Shared.Domain.Tickets.Events;
using Shared.Notifications;

namespace Api.Notifications;

public sealed class TicketAssignedNotificationHandler
    : INotificationHandler<TicketAssignedDomainEvent>
{
    private readonly SupportFlowDbContext _db;
    private readonly INotificationService _notificationService;

    public TicketAssignedNotificationHandler(
        SupportFlowDbContext db,
        INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }

    public async Task Handle(
        TicketAssignedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == notification.TicketId,
                cancellationToken);

        if (ticket is null)
        {
            return;
        }

        var newNotification = Notification.Create(
            notification.AssignedToUserId,
            notification.CompanyId,
            notification.TicketId,
            NotificationType.TicketAssigned,
            "Ticket Assigned",
            $"Ticket #{ticket.TicketNumber} has been assigned to you.");

        _db.Notifications.Add(newNotification);

        await _db.SaveChangesAsync(cancellationToken);

        await _notificationService.SendAsync(
            notification.AssignedToUserId,
            newNotification,
            cancellationToken);
    }
}