using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Dashboard.Unassigned;

public sealed class UnassignedTicketsCacheKeyProvider
    : ICacheKeyProvider<GetUnassignedTicketsQuery>
{
    private readonly ICurrentUser _currentUser;

    public UnassignedTicketsCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
        GetUnassignedTicketsQuery request)
    {
        var scope = request.CompanyId?.ToString() ?? "all";
        var limit = Math.Clamp(request.Limit, 1, 20);
        return $"dashboard:unassigned:{scope}:user:{_currentUser.UserId}:role:{_currentUser.Role}:limit:{limit}";
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