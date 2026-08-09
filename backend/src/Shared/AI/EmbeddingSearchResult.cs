namespace Shared.AI;

public sealed record EmbeddingSearchResult(
    Guid SourceId,
    string Content,
    double Distance);