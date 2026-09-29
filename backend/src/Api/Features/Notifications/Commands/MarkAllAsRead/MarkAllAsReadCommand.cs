using MediatR;

namespace Api.Features.Notifications.Commands.MarkAllAsRead;

public sealed record MarkAllAsReadCommand
    : IRequest;