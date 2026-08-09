using MediatR;
using Shared.AI;
using Shared.Domain.AI;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Event.Handler;

public sealed class KnowledgeArticleUpdatedDomainEventHandler
    : INotificationHandler<KnowledgeArticleUpdatedDomainEvent>
{
    private const string SourceType = "KnowledgeArticle";

    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;

    public KnowledgeArticleUpdatedDomainEventHandler(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
    }

    public async Task Handle(
        KnowledgeArticleUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var text = BuildEmbeddingText(
            notification.Title,
            notification.Content);

        var vector =
            await _embeddingService.GenerateAsync(
                text,
                cancellationToken);

        var document = new EmbeddingDocument(
            notification.ArticleId,
            SourceType,
            text,
            vector,
            notification.CompanyId);

        await _vectorStore.UpsertAsync(
            document,
            cancellationToken);
    }


    private static string BuildEmbeddingText(
        string title,
        string content)
    {
        return $"""
            Knowledge Article Title:
            {title}

            Knowledge Article Content:
            {content}
            """;
    }
}