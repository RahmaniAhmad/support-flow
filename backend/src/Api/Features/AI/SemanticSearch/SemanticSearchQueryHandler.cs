using MediatR;
using Shared.AI;
using Shared.Authentication;

namespace Api.Features.AI.SemanticSearch;

public sealed class SemanticSearchQueryHandler
    : IRequestHandler<
        SemanticSearchQuery,
        SemanticSearchResponse>
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly ICurrentUser _currentUser;

    public SemanticSearchQueryHandler(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ICurrentUser currentUser)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _currentUser = currentUser;
    }

    public async Task<SemanticSearchResponse> Handle(
        SemanticSearchQuery request,
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
            .Select(x => new SemanticSearchResult(
                x.SourceId,
                x.SourceType,
                x.Content,
                x.Distance))
            .ToList();

        return new SemanticSearchResponse(results);
    }
}