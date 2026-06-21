using Shared.Caching;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsCacheKeyProvider
    : ICacheKeyProvider<GetTicketsQuery>
{
    public string GetBaseKey(GetTicketsQuery request)
        => $"tickets:company:{request.CompanyId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
