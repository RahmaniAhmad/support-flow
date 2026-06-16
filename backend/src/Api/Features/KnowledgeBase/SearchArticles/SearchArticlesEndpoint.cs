using MediatR;

namespace Api.Features.KnowledgeBase.SearchArticles;

public static class SearchArticlesEndpoint
{
    public static IEndpointRouteBuilder MapSearchArticles(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/knowledge-articles/search",
            async (
                string query,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(query))
                    return Results.BadRequest("Query cannot be empty.");

                var result = await sender.Send(
                    new SearchArticlesQuery(query),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization();

        return app;
    }
}
