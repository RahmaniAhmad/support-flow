using MediatR;
using Shared.AI;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Event.Handler;

public sealed class KnowledgeArticleDeletedDomainEventHandler
    : INotificationHandler<KnowledgeArticleDeletedDomainEvent>
{
    private const string SourceType = "KnowledgeArticle";

    private readonly IVectorStore _vectorStore;

    public KnowledgeArticleDeletedDomainEventHandler(
        IVectorStore vectorStore)
    {
        _vectorStore = vectorStore;
    }

    public async Task Handle(
        KnowledgeArticleDeletedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _vectorStore.DeleteAsync(
            notification.ArticleId,
            SourceType,
            cancellationToken);
    }
}