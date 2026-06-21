using Shared.Caching;

namespace Api.Features.Tickets.GetMyTickets;

public sealed class GetMyTicketsCacheKeyProvider
    : ICacheKeyProvider<GetMyTicketsQuery>
{
    public string GetBaseKey(GetMyTicketsQuery request)
        => $"tickets:company:{request.CompanyId}:user:{request.UserId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
