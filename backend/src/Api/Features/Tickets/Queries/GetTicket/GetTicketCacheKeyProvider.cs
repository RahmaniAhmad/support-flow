using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Tickets.Queries.GetTicket;

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
        => $"tickets:user:{_currentUser.UserId}:company:{_currentUser.CompanyId?.ToString() ?? "all"}:role:{_currentUser.Role}:ticket:{request.TicketId}";

    public TimeSpan? Expiration => CacheExpiration.Ticket;

    public string GetGroup(GetTicketQuery request)
    {
        return _currentUser.CompanyId is Guid companyId
            ? $"tickets:company:{companyId}"
            : "tickets:all";
    }
}
