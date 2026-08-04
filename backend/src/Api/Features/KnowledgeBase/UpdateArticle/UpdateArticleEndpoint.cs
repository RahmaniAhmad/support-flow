using Api.Filters;
using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.KnowledgeBase.UpdateArticle;

public static class UpdateArticleEndpoint
{
    public static IEndpointRouteBuilder MapUpdateArticle(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/knowledge-articles/{id:guid}",
            async (
                Guid id,
                UpdateArticleRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var updated = await sender.Send(
                    new UpdateArticleCommand(
                        id,
                        request.Title,
                        request.Content),
                        cancellationToken);

                return updated
                    ? Results.Ok()
                    : Results.NotFound();
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization().RequirePermission(Permissions.KnowledgeArticlesUpdate);

        return app;
    }
}
