using Shared.Caching;

namespace Api.Features.Tickets.GetUnassignedTickets;

public sealed class GetUnassignedTicketsCacheKeyProvider
    : ICacheKeyProvider<GetUnassignedTicketsQuery>
{
    public string GetBaseKey(GetUnassignedTicketsQuery request)
        => $"tickets:company:{request.CompanyId}:unassigned";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
