using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Notifications;
using Shared.Domain.Tickets.Events;
using Shared.Domain.Users;
using Shared.Notifications;

namespace Api.Features.Notifications.EventHandlers;

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
        var ticket =
            await _db.Tickets
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == notification.TicketId,
                    cancellationToken);


        var assignedByUser =
            await _db.Users
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == notification.AssignedByUserId,
                    cancellationToken);


        var recipients = new List<Guid>
        {
            // Customer always knows
            ticket.CreatedByUserId
        };


        // Admin assignment -> notify agent
        if (assignedByUser.Role == UserRole.Admin ||
            assignedByUser.Role == UserRole.SuperAdmin)
        {
            recipients.Add(
                notification.AssignedToUserId);
        }


        // Do not notify the actor
        recipients.Remove(
            notification.AssignedByUserId);


        foreach (var userId in recipients.Distinct())
        {
            var newNotification =
                Notification.Create(
                    userId,
                    notification.CompanyId,
                    notification.TicketId,
                    NotificationType.TicketAssigned,
                    "Ticket Assigned",
                    $"Ticket #{ticket.TicketNumber} has been assigned to you.");


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