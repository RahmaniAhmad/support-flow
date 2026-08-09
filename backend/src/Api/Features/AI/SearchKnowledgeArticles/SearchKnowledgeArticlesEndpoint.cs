using MediatR;

namespace Api.Features.AI.SearchKnowledgeArticles;

public static class SearchKnowledgeArticlesEndpoint
{
    public static IEndpointRouteBuilder MapSearchKnowledgeArticles(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/api/ai/search-knowledge",
            async (
                SearchKnowledgeArticlesRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new SearchKnowledgeArticlesQuery(
                    request.Query,
                    request.Limit);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return Results.Ok(result);
            });

        return endpoints;
    }
}