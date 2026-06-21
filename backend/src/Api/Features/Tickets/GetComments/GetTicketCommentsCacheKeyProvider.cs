using Shared.Caching;

namespace Api.Features.Tickets.GetComments;

public sealed class GetTicketCommentsCacheKeyProvider
    : ICacheKeyProvider<GetTicketCommentsQuery>
{
    public string GetBaseKey(GetTicketCommentsQuery request)
        => $"tickets:comments:{request.TicketId}";

    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}
