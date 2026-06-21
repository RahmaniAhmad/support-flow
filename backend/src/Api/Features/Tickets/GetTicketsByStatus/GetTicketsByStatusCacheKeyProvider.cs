using Shared.Caching;

namespace Api.Features.Tickets.GetTicketsByStatus;

public sealed class GetTicketsByStatusCacheKeyProvider
    : ICacheKeyProvider<GetTicketsByStatusQuery>
{
    public string GetBaseKey(GetTicketsByStatusQuery request)
        => $"tickets:company:{request.CompanyId}:status:{request.Status}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
