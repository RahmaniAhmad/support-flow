using Shared.Domain.Base;

namespace Shared.Domain.KnowledgeBase.Events;

public sealed record KnowledgeArticleDeletedDomainEvent(
    Guid ArticleId,
    Guid? CompanyId) : IDomainEvent;