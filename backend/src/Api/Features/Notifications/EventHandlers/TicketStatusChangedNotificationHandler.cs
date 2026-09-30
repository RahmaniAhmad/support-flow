using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Notifications;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Users;
using Shared.Notifications;

namespace Api.Features.Notifications.EventHandlers;

public sealed class TicketStatusChangedNotificationHandler
    : INotificationHandler<TicketStatusChangedDomainEvent>
{
    private readonly SupportFlowDbContext _db;
    private readonly INotificationService _notificationService;


    public TicketStatusChangedNotificationHandler(
        SupportFlowDbContext db,
        INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }


    public async Task Handle(
        TicketStatusChangedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var ticket =
            await _db.Tickets
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == notification.TicketId,
                    cancellationToken);


        var changedByUser =
            await _db.Users
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == notification.ChangedByUserId,
                    cancellationToken);


        var recipients = new List<Guid>
        {
            // Customer always knows
            ticket.CreatedByUserId
        };


        // Admin changes status -> agent should know
        if (changedByUser.Role == UserRole.Admin ||
            changedByUser.Role == UserRole.SuperAdmin)
        {
            if (ticket.AssignedToUserId.HasValue)
            {
                recipients.Add(
                    ticket.AssignedToUserId.Value);
            }
        }


        // Remove the person who changed status
        recipients.Remove(
            notification.ChangedByUserId);


        foreach (var userId in recipients.Distinct())
        {
            var newNotification =
                Notification.Create(
                    userId,
                    notification.CompanyId,
                    notification.TicketId,
                    NotificationType.TicketStatusChanged,
                    "Ticket Status Changed",
                    $"Ticket #{ticket.TicketNumber} changed from {notification.OldStatus} to {notification.NewStatus}.");


            _db.Notifications.Add(
                newNotification);


            await _notificationService.SendAsync(
                userId,
                newNotification,
                cancellationToken);
        }


        await _db.SaveChangesAsync(
            cancellationToken);
    }
}