namespace Shared.AI;

public sealed record EmbeddingSearchResult(
    Guid SourceId,
    string SourceType,
    string Content,
    double Distance);