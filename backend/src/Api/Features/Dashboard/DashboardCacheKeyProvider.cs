using Shared.Caching;

namespace Api.Features.Dashboard;

public sealed class DashboardCacheKeyProvider
    : ICacheKeyProvider<DashboardQuery>
{
    public string GetKey(
       DashboardQuery request)
    {
        return request.CompanyId.HasValue
         ? $"dashboard:{request.CompanyId}"
         : "dashboard:all";
    }


    public string GetGroup(
        DashboardQuery request)
    {
        return request.CompanyId.HasValue
           ? $"dashboard:{request.CompanyId}"
           : "dashboard:all";
    }

    public TimeSpan? Expiration => CacheExpiration.Dashboard;
}