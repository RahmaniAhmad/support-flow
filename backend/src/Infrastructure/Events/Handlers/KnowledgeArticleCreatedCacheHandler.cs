using MediatR;
using Shared.Caching;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Events.Handlers;

public sealed class KnowledgeArticleCreatedCacheHandler
    : INotificationHandler<KnowledgeArticleCreatedDomainEvent>
{
    private readonly ICacheService _cache;

    public KnowledgeArticleCreatedCacheHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        KnowledgeArticleCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"knowledge-articles:company:{notification.CompanyId}:article:{notification.ArticleId}:version");
    }
}