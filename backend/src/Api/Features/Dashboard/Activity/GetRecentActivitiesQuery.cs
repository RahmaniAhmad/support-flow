using MediatR;

namespace Api.Features.Dashboard.Activity;

public sealed record GetRecentActivitiesQuery(
    Guid? CompanyId,
    int Limit = 10
)
: IRequest<IReadOnlyList<RecentActivityResponse>>;