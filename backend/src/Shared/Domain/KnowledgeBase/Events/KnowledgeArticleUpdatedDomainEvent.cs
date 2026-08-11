using Shared.Domain.Base;

namespace Shared.Domain.KnowledgeBase.Events;

public sealed record KnowledgeArticleUpdatedDomainEvent(
    Guid ArticleId,
    Guid CompanyId,
    string Title,
    string Content) : IDomainEvent;

