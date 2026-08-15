using MediatR;

namespace Api.Features.Dashboard.Statistics;

public sealed record GetDashboardStatisticsQuery(
    Guid? CompanyId
) : IRequest<DashboardStatisticsResponse>;