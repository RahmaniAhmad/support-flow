using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Dashboard.Agents;

public sealed class AgentPerformanceCacheKeyProvider
    : ICacheKeyProvider<GetAgentPerformanceQuery>
{
    private readonly ICurrentUser _currentUser;

    public AgentPerformanceCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
        GetAgentPerformanceQuery request)
    {
        var scope = request.CompanyId?.ToString() ?? "all";
        return $"dashboard:agents:{scope}:user:{_currentUser.UserId}:role:{_currentUser.Role}";
    }

    public string GetGroup(
        GetAgentPerformanceQuery request)
    {
        return request.CompanyId.HasValue
        ? $"dashboard:agents:{request.CompanyId}"
        : "dashboard:agents:all";
    }

    public TimeSpan? Expiration =>
        CacheExpiration.Dashboard;
}