using Shared.Domain.Base;

namespace Shared.Domain.KnowledgeBase.Events;

public sealed record KnowledgeArticleCreatedDomainEvent(
    Guid ArticleId,
    Guid CompanyId,
    string Title,
    string Content) : IDomainEvent;

