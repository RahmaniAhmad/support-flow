using MediatR;
using Microsoft.Extensions.Logging;
using Shared.AI;
using Shared.Domain.AI;
using Shared.Domain.KnowledgeBase.Events;

namespace Infrastructure.Events.Handlers;

public sealed class KnowledgeArticleUpdatedEmbeddingHandler
    : INotificationHandler<KnowledgeArticleUpdatedDomainEvent>
{
    private const string SourceType = "KnowledgeArticle";

    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly ILogger<KnowledgeArticleUpdatedEmbeddingHandler> _logger;

    public KnowledgeArticleUpdatedEmbeddingHandler(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ILogger<KnowledgeArticleUpdatedEmbeddingHandler> logger)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _logger = logger;
    }

    public async Task Handle(
        KnowledgeArticleUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to generate embedding for KnowledgeArticle {ArticleId}",
                notification.ArticleId);
        }
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