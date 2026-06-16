using MediatR;

namespace Api.Features.KnowledgeBase.GetArticle;

public static class GetArticleEndpoint
{
    public static IEndpointRouteBuilder MapGetArticle(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/knowledge-articles/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var article = await sender.Send(
                    new GetArticleQuery(id),
                    cancellationToken);

                return article is null
                    ? Results.NotFound()
                    : Results.Ok(article);
            })
            .RequireAuthorization();

        return app;
    }
}
