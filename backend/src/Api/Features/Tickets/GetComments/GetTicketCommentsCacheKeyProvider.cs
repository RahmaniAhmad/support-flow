using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.Tickets.GetComments;

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
        => $"tickets:comments:{request.TicketId}";


    public string GetGroup(
        GetTicketCommentsQuery request)
    {
        var companyId = _currentUser.CompanyId
            ?? throw new InvalidOperationException(
                "Company id is required.");

        return $"tickets:company:{companyId}:ticket:{request.TicketId}";
    }


    public TimeSpan? Expiration => CacheExpiration.Comments;
}