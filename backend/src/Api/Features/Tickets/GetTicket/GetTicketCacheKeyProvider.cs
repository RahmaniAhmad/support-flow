using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Tickets.GetTicket;

public sealed class GetTicketCacheKeyProvider
    : ICacheKeyProvider<GetTicketQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetTicketCacheKeyProvider(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
        GetTicketQuery request)
    {
        var companyId = _currentUser.CompanyId
            ?? throw new InvalidOperationException(
                "Company id is required.");

        return $"tickets:company:{companyId}:ticket:{request.TicketId}";
    }

    public TimeSpan? Expiration => CacheExpiration.Ticket;
}
