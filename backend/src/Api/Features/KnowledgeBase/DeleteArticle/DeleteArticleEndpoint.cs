using MediatR;

namespace Api.Features.KnowledgeBase.DeleteArticle;

public static class DeleteArticleEndpoint
{
    public static IEndpointRouteBuilder MapDeleteArticle(
        this IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/knowledge-articles/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var deleted = await sender.Send(
                    new DeleteArticleCommand(id),
                    cancellationToken);

                return deleted
                    ? Results.NoContent()
                    : Results.NotFound();
            })
            .RequireAuthorization();

        return app;
    }
}
