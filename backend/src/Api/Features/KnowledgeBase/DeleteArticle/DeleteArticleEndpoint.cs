using Api.Filters;
using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

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
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization().RequirePermission(Permissions.KnowledgeArticlesDelete);

        return app;
    }
}
