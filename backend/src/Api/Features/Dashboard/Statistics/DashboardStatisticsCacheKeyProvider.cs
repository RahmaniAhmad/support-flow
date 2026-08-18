using Shared.Caching;

namespace Api.Features.Dashboard.Statistics;


public sealed class DashboardStatisticsCacheKeyProvider
    : ICacheKeyProvider<GetDashboardStatisticsQuery>
{

    public string GetKey(
        GetDashboardStatisticsQuery request)
    {
        return request.CompanyId.HasValue
            ? $"dashboard:statistics:{request.CompanyId}"
            : "dashboard:statistics:all";
    }



    public string GetGroup(
        GetDashboardStatisticsQuery request)
    {
        return "dashboard:statistics";
    }



    public TimeSpan? Expiration =>
        CacheExpiration.Dashboard;
}