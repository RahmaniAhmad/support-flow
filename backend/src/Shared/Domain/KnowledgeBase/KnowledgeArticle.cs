using Shared.Domain.Base;
using Shared.Domain.KnowledgeBase.Events;

namespace Shared.Domain.KnowledgeBase;

public sealed class KnowledgeArticle : AggregateRoot
{
    public Guid CompanyId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }
    private KnowledgeArticle()
    {
    }

    public static KnowledgeArticle Create(
        Guid companyId,
        string title,
        string content)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
               companyId,
               Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        var article = new KnowledgeArticle
        {
            CompanyId = companyId,
            Title = title.Trim(),
            Content = content.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        article.AddDomainEvent(
            new KnowledgeArticleCreatedDomainEvent(
                article.Id,
                article.CompanyId,
                article.Title,
                article.Content));

        return article;
    }


    public void Update(
        string title,
        string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        Title = title.Trim();
        Content = content.Trim();
        UpdatedAtUtc = DateTime.UtcNow;


        AddDomainEvent(
            new KnowledgeArticleUpdatedDomainEvent(
                Id,
                CompanyId,
                Title,
                content));
    }


    public void Delete()
    {
        AddDomainEvent(
            new KnowledgeArticleDeletedDomainEvent(
                Id,
                CompanyId));
    }

}