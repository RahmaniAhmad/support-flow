using MediatR;

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery(
    int Page,
    int PageSize)
    : IRequest<List<GetNotificationsResponse>>;