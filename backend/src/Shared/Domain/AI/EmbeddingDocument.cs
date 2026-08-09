
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

    public EmbeddingDocument(
        Guid sourceId,
        string sourceType,
        string content,
        Vector vector,
        Guid companyId)
    {
        Id = Guid.NewGuid();
        SourceId = sourceId;
        SourceType = sourceType;
        Content = content;
        Vector = vector;
        CompanyId = companyId;
    }
}