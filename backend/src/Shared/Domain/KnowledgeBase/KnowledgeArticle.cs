using Shared.Domain.Base;
using Shared.Domain.KnowledgeBase.Events;

namespace Shared.Domain.KnowledgeBase;

public sealed class KnowledgeArticle : AggregateRoot
{
    public Guid? CompanyId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }
    private KnowledgeArticle()
    {
    }

    public static KnowledgeArticle Create(
        Guid? companyId,
        string title,
        string content)
    {
        if (companyId == Guid.Empty)
            throw new ArgumentException(
                "Company id cannot be empty.",
                nameof(companyId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title is required.",
                nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Content is required.",
                nameof(content));


        var article = new KnowledgeArticle
        {
            CompanyId = companyId,
            Title = title,
            Content = content,
            CreatedAtUtc = DateTime.UtcNow
        };

        article.AddDomainEvent(
            new KnowledgeArticleCreatedDomainEvent(
                article.Id,
                article.CompanyId,
                article.Title));

        return article;
    }


    public void Update(
        string title,
        string content)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Title is required.",
                nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException(
                "Content is required.",
                nameof(content));


        Title = title;
        Content = content;
        UpdatedAtUtc = DateTime.UtcNow;


        AddDomainEvent(
            new KnowledgeArticleUpdatedDomainEvent(
                Id,
                CompanyId,
                Title));
    }


    public void Delete()
    {
        AddDomainEvent(
            new KnowledgeArticleDeletedDomainEvent(
                Id,
                CompanyId));
    }

}