namespace Api.Features.AI.SemanticSearch;

public sealed record SemanticSearchResponse(
    IReadOnlyList<SemanticSearchResult> Results);

public sealed record SemanticSearchResult(
    Guid ArticleId,
    string SourceType,
    string Title,
    string Content,
    double Distance);