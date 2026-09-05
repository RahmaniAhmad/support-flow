using Shared.Caching;

namespace Api.Features.Dashboard.Activity;

public sealed class RecentActivitiesCacheKeyProvider
    : ICacheKeyProvider<GetRecentActivitiesQuery>
{

    public string GetKey(
        GetRecentActivitiesQuery request)
    {

        return request.CompanyId.HasValue
            ? $"dashboard:activities:{request.CompanyId}"
            : "dashboard:activities:all";
    }

    public string GetGroup(
        GetRecentActivitiesQuery request)
    {
        return request.CompanyId.HasValue
        ? $"dashboard:activities:{request.CompanyId}"
        : "dashboard:activities:all";
    }

    public TimeSpan? Expiration =>
        CacheExpiration.Dashboard;
}