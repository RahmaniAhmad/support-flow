using Shared.Domain.Notifications;

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsResponse(
    Guid Id,
    Guid? TicketId,
    NotificationType Type,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAtUtc);