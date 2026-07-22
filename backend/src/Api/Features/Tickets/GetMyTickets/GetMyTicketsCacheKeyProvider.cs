using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Tickets.GetMyTickets;

public sealed class GetMyTicketsCacheKeyProvider
    : ICacheKeyProvider<GetMyTicketsQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetMyTicketsCacheKeyProvider(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(
     GetMyTicketsQuery request)
    {
        return $"tickets:user:{_currentUser.UserId}:my";
    }

    public string GetGroup(
        GetMyTicketsQuery request)
    {
        return $"tickets:user:{_currentUser.UserId}";
    }

    public TimeSpan? Expiration => CacheExpiration.Ticket;
}
