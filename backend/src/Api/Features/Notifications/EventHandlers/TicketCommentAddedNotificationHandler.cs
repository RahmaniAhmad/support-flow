using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Notifications;
using Shared.Domain.Tickets.Events;
using Shared.Notifications;

namespace Api.Features.Notifications.EventHandlers;

public sealed class TicketCommentAddedNotificationHandler
    : INotificationHandler<TicketCommentAddedDomainEvent>
{
    private readonly SupportFlowDbContext _db;
    private readonly INotificationService _notificationService;


    public TicketCommentAddedNotificationHandler(
        SupportFlowDbContext db,
        INotificationService notificationService)
    {
        _db = db;
        _notificationService = notificationService;
    }


    public async Task Handle(
        TicketCommentAddedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var ticket =
            await _db.Tickets
                .AsNoTracking()
                .FirstAsync(
                    x => x.Id == notification.TicketId,
                    cancellationToken);

        var commenter = await _db.Users
            .AsNoTracking()
            .FirstAsync(
                x => x.Id == notification.CommentedByUserId,
                cancellationToken);


        var recipients = new List<Guid>
        {
            ticket.CreatedByUserId
        };


        if (ticket.AssignedToUserId.HasValue)
        {
            recipients.Add(
                ticket.AssignedToUserId.Value);
        }


        recipients =
            recipients
                .Where(x =>
                    x != notification.CommentedByUserId)
                .Distinct()
                .ToList();



        foreach (var userId in recipients)
        {
            var newNotification =
                Notification.Create(
                    userId,
                    notification.CompanyId,
                    ticket.Id,
                    NotificationType.TicketCommentAdded,
                    "New Comment",
                    $"{commenter.FirstName} {commenter.LastName} added a comment to ticket #{ticket.TicketNumber}");


            _db.Notifications.Add(newNotification);


            await _notificationService.SendAsync(
                userId,
                newNotification,
                cancellationToken);
        }


        await _db.SaveChangesAsync(
            cancellationToken);
    }
}