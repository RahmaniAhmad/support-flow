using MediatR;
using Shared.AI;
using Shared.Authentication;

namespace Api.Features.AI.SearchKnowledgeArticles;

public sealed class SearchKnowledgeArticlesQueryHandler
    : IRequestHandler<
        SearchKnowledgeArticlesQuery,
        SearchKnowledgeArticlesResponse>
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly ICurrentUser _currentUser;

    public SearchKnowledgeArticlesQueryHandler(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ICurrentUser currentUser)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _currentUser = currentUser;
    }

    public async Task<SearchKnowledgeArticlesResponse> Handle(
        SearchKnowledgeArticlesQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        if (!companyId.HasValue)
        {
            throw new InvalidOperationException(
                "Current user is not associated with a company.");
        }

        var vector =
            await _embeddingService.GenerateAsync(
                request.Query,
                cancellationToken);

        var documents =
            await _vectorStore.SearchAsync(
                vector,
                companyId.Value,
                request.Limit,
                cancellationToken);

        var results = documents
            .Select(x => new SearchKnowledgeArticleResult(
                x.SourceId,
                x.Content,
                x.Distance))
            .ToList();

        return new SearchKnowledgeArticlesResponse(results);
    }
}