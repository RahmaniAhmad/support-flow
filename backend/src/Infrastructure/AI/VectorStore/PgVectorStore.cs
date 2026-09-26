using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Shared.AI;
using Shared.Domain.AI;

namespace Infrastructure.AI.VectorStore;

public sealed class PgVectorStore : IVectorStore
{
    private readonly SupportFlowDbContext _db;

    public PgVectorStore(SupportFlowDbContext db)
    {
        _db = db;
    }


    public async Task UpsertAsync(
        EmbeddingDocument document,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(document);

        var existing =
            await _db.EmbeddingDocuments
                .SingleOrDefaultAsync(
                    x =>
                        x.SourceId == document.SourceId &&
                        x.SourceType == document.SourceType &&
                        x.CompanyId == document.CompanyId,
                    cancellationToken);

        if (existing is not null)
        {
            _db.EmbeddingDocuments.Remove(existing);
        }

        _db.EmbeddingDocuments.Add(document);

        await _db.SaveChangesAsync(
            cancellationToken);
    }


    public async Task DeleteAsync(
        Guid sourceId,
        string sourceType,
        Guid companyId,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
        sourceId,
        Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(sourceType);

        await _db.EmbeddingDocuments
            .Where(x =>
                x.SourceId == sourceId &&
                x.SourceType == sourceType &&
                x.CompanyId == companyId)
            .ExecuteDeleteAsync(
                cancellationToken);
    }


    public async Task<List<EmbeddingSearchResult>> SearchAsync(
        Vector vector,
        Guid companyId,
        int limit,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(vector);

        ArgumentOutOfRangeException.ThrowIfEqual(
            companyId,
            Guid.Empty);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);


        return await _db.EmbeddingDocuments
            .Where(x =>
                x.CompanyId == companyId &&
                  x.Vector.CosineDistance(vector) < 0.40)
            .OrderBy(x =>
                x.Vector.CosineDistance(vector))
            .Select(x => new EmbeddingSearchResult(
            x.SourceId,
            x.SourceType,
            x.Content,
            x.Vector.CosineDistance(vector)))
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}