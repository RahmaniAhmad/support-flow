using Pgvector;
using Shared.Domain.AI;

namespace Shared.AI;

public interface IVectorStore
{
    Task UpsertAsync(
          EmbeddingDocument document,
          CancellationToken cancellationToken);

    Task DeleteAsync(
        Guid sourceId,
        string sourceType,
        CancellationToken cancellationToken);

    Task<List<EmbeddingSearchResult>> SearchAsync(
        Vector vector,
        Guid companyId,
        int limit,
        CancellationToken cancellationToken);

}