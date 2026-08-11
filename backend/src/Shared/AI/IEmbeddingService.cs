using Pgvector;

namespace Shared.AI;

public interface IEmbeddingService
{
    Task<Vector> GenerateAsync(
        string text,
        CancellationToken cancellationToken);
}