namespace Shared.AI;

public sealed record EmbeddingSearchResult(
    Guid SourceId,
    string SourceType,
    string Title,
    string Content,
    double Distance);