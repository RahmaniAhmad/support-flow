namespace Api.Features.KnowledgeBase.SearchArticles;

public sealed record SearchArticlesResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAtUtc);