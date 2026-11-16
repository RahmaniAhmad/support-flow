using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Tickets.Queries.GetComments;

public sealed class GetTicketCommentsCacheKeyProvider
    : ICacheKeyProvider<GetTicketCommentsQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetTicketCommentsCacheKeyProvider(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }


    public string GetKey(
        GetTicketCommentsQuery request)
        => $"tickets:comments:user:{_currentUser.UserId}:company:{_currentUser.CompanyId?.ToString() ?? "all"}:role:{_currentUser.Role}:{request.TicketId}";


    public string GetGroup(
        GetTicketCommentsQuery request)
    {
        return _currentUser.CompanyId is Guid companyId
            ? $"tickets:company:{companyId}:ticket:{request.TicketId}"
            : $"tickets:all:ticket:{request.TicketId}";
    }


    public TimeSpan? Expiration => CacheExpiration.Comments;
}