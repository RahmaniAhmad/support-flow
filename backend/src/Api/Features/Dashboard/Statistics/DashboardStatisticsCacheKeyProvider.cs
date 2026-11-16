using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Dashboard.Statistics;


public sealed class DashboardStatisticsCacheKeyProvider
    : ICacheKeyProvider<GetDashboardStatisticsQuery>
{
    private readonly ICurrentUser _currentUser;

    public DashboardStatisticsCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
        GetDashboardStatisticsQuery request)
    {
        var scope = request.CompanyId?.ToString() ?? "all";
        return $"dashboard:statistics:{scope}:user:{_currentUser.UserId}:role:{_currentUser.Role}";
    }



    public string GetGroup(
        GetDashboardStatisticsQuery request)
    {
        return request.CompanyId.HasValue
      ? $"dashboard:statistics:{request.CompanyId}"
      : "dashboard:statistics:all";
    }



    public TimeSpan? Expiration =>
        CacheExpiration.Dashboard;
}