namespace Api.Features.KnowledgeBase.GetArticles;

public sealed record GetArticlesResponse(
    Guid Id,
    string Title,
    DateTime CreatedAtUtc);