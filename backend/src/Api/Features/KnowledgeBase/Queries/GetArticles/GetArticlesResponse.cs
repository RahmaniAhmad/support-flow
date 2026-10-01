namespace Api.Features.KnowledgeBase.Queries.GetArticles;

public sealed record GetArticlesResponse(
    Guid Id,
    string Title,
    DateTime CreatedAtUtc);