using MediatR;
using Shared.Caching;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Events.Handlers;

public sealed class KnowledgeArticleUpdatedCacheHandler
    : INotificationHandler<KnowledgeArticleUpdatedDomainEvent>
{
    private readonly ICacheService _cache;

    public KnowledgeArticleUpdatedCacheHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        KnowledgeArticleUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"knowledge-articles:company:{notification.CompanyId}:article:{notification.ArticleId}:version");
    }
}