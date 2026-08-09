namespace Api.Features.AI.SearchKnowledgeArticles;

public sealed record SearchKnowledgeArticlesResponse(
    IReadOnlyList<SearchKnowledgeArticleResult> Results);

public sealed record SearchKnowledgeArticleResult(
    Guid ArticleId,
    string Content,
    double Distance);