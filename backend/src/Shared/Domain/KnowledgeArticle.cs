namespace Shared.Domain;

public sealed class KnowledgeArticle
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;
}