using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Dashboard.Activity;

public sealed class RecentActivitiesCacheKeyProvider
    : ICacheKeyProvider<GetRecentActivitiesQuery>
{
    private readonly ICurrentUser _currentUser;

    public RecentActivitiesCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
        GetRecentActivitiesQuery request)
    {
        var scope = request.CompanyId?.ToString() ?? "all";
        var limit = Math.Clamp(request.Limit, 1, 20);
        return $"dashboard:activities:{scope}:user:{_currentUser.UserId}:role:{_currentUser.Role}:limit:{limit}";
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