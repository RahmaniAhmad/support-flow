using MediatR;

namespace Api.Features.KnowledgeBase.GetArticles;

public static class GetArticlesEndpoint
{
    public static IEndpointRouteBuilder MapGetArticles(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/knowledge-articles",
            async (ISender sender,
                CancellationToken cancellationToken) =>
            {
                var articles = await sender.Send(
                    new GetArticlesQuery(),
                    cancellationToken);

                return Results.Ok(articles);
            })
            .RequireAuthorization();

        return app;
    }
}
