namespace Api.Features.KnowledgeBase.Queries.SearchArticles;

public sealed record SearchArticlesResponse(
    Guid Id,
    string Title,
    string Content,
    DateTime CreatedAtUtc);