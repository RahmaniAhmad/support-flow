using Shared.Caching;
namespace Api.Features.Dashboard.Agents;

public sealed class AgentPerformanceCacheKeyProvider
    : ICacheKeyProvider<GetAgentPerformanceQuery>
{
    public string GetKey(
        GetAgentPerformanceQuery request)
    {
        return request.CompanyId.HasValue
            ? $"dashboard:agents:{request.CompanyId}"
            : "dashboard:agents:all";
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