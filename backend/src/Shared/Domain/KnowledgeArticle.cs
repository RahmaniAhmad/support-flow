namespace Shared.Domain;

public sealed class KnowledgeArticle
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}