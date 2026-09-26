using Pgvector;

namespace Shared.Domain.AI;

public sealed class EmbeddingDocument
{
    public Guid Id { get; private set; }

    public Guid SourceId { get; private set; }

    public string SourceType { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public Vector Vector { get; private set; } = default!;

    public Guid CompanyId { get; private set; }

    private EmbeddingDocument()
    {
    }

    public static EmbeddingDocument Create(
        Guid sourceId,
        string sourceType,
        string content,
        Vector vector,
        Guid companyId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            sourceId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            sourceType);

        ArgumentException.ThrowIfNullOrWhiteSpace(
            content);

        ArgumentNullException.ThrowIfNull(vector);

        ArgumentOutOfRangeException.ThrowIfEqual(
            companyId,
            Guid.Empty);

        return new EmbeddingDocument
        {
            Id = Guid.NewGuid(),
            SourceId = sourceId,
            SourceType = sourceType.Trim(),
            Content = content.Trim(),
            Vector = vector,
            CompanyId = companyId
        };
    }
}