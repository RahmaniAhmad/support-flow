namespace Api.Features.AI.GetTicketSuggestedArticles;

public sealed record GetTicketSuggestedArticlesResponse(
    IReadOnlyList<SuggestedArticleResponse> Results);

public sealed record SuggestedArticleResponse(
    Guid ArticleId,
    string Content,
    double Distance);