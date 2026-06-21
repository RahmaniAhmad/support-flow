using Shared.Caching;

namespace Api.Features.Dashboard;

public sealed class DashboardCacheKeyProvider
    : ICacheKeyProvider<DashboardQuery>
{
    public string GetBaseKey(DashboardQuery request)
       => $"dashboard:{request.CompanyId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}