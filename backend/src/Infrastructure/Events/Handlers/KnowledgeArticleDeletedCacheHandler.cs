using MediatR;
using Shared.Caching;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Events.Handlers;

public sealed class KnowledgeArticleDeletedCacheHandler
    : INotificationHandler<KnowledgeArticleDeletedDomainEvent>
{
    private readonly ICacheService _cache;

    public KnowledgeArticleDeletedCacheHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        KnowledgeArticleDeletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"knowledge-articles:company:{notification.CompanyId}:article:{notification.ArticleId}:version");
    }
}