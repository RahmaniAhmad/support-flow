namespace Api.Features.KnowledgeBase.GetArticle;

public sealed record GetArticleResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAtUtc);