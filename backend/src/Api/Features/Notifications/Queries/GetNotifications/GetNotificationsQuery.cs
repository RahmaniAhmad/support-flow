using MediatR;
using Shared.Contracts;

namespace Api.Features.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery(
    int Page,
    int PageSize)
    : IRequest<PagedResult<GetNotificationsResponse>>;