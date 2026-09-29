using MediatR;

namespace Api.Features.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand(
    Guid NotificationId)
    : IRequest;