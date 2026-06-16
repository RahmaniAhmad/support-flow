using Shared.Domain.Base;

namespace Shared.Domain;

public sealed class KnowledgeArticle : AggregateRoot
{
    public Guid CompanyId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}