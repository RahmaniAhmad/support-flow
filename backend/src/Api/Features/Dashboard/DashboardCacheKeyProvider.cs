using Shared.Caching;

namespace Api.Features.Dashboard;

public sealed class DashboardCacheKeyProvider
    : ICacheKeyProvider<DashboardQuery>
{
    public string GetKey(
       DashboardQuery request)
    {
        if (request.CompanyId is null)
            throw new InvalidOperationException(
                "SuperAdmin dashboard cannot be cached by company.");

        return $"dashboard:{request.CompanyId}";
    }


    public string GetGroup(
        DashboardQuery request)
    {
        if (request.CompanyId is null)
            throw new InvalidOperationException(
                "SuperAdmin dashboard cannot be cached by company.");

        return $"dashboard:{request.CompanyId}";
    }

    public TimeSpan? Expiration => CacheExpiration.Dashboard;
}