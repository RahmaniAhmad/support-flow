using MediatR;

namespace Api.Features.Notifications.Queries.GetUnreadCount;

public sealed record GetUnreadCountQuery
    : IRequest<GetUnreadCountResponse>;