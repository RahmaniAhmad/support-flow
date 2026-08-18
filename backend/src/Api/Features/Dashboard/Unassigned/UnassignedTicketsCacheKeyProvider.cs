using Shared.Caching;

namespace Api.Features.Dashboard.Unassigned;

public sealed class UnassignedTicketsCacheKeyProvider
    : ICacheKeyProvider<GetUnassignedTicketsQuery>
{
    public string GetKey(
        GetUnassignedTicketsQuery request)
    {
        return request.CompanyId.HasValue
            ? $"dashboard:unassigned:{request.CompanyId}:{request.Limit}"
            : $"dashboard:unassigned:all:{request.Limit}";
    }

    public string GetGroup(
        GetUnassignedTicketsQuery request)
    {
        return request.CompanyId.HasValue
            ? $"dashboard:unassigned:{request.CompanyId}"
            : "dashboard:unassigned:all";
    }

    public TimeSpan? Expiration =>
        CacheExpiration.Dashboard;
}