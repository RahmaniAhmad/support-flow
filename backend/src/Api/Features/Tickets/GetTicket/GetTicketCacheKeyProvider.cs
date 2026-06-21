using Shared.Caching;

namespace Api.Features.Tickets.GetTicket;

public sealed class GetTicketCacheKeyProvider
    : ICacheKeyProvider<GetTicketQuery>
{
    public string GetBaseKey(GetTicketQuery request)
        => $"tickets:company:{request.CompanyId}:ticket:{request.TicketId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
