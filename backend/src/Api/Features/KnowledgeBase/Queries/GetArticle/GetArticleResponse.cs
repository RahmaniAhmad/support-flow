namespace Api.Features.KnowledgeBase.Queries.GetArticle;

public sealed record GetArticleResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAtUtc);