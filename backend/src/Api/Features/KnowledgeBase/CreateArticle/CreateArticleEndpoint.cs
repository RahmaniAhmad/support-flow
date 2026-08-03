using Api.Filters;
using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.KnowledgeBase.CreateArticle;

public static class CreateArticleEndpoint
{
    public static IEndpointRouteBuilder MapCreateArticle(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/knowledge-articles",
            async (
                CreateArticleRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var articleId = await sender.Send(
                    new CreateArticleCommand(
                        request.Title,
                        request.Content),
                        cancellationToken);

                return Results.Created(
                    $"/knowledge-articles/{articleId}",
                    new { Id = articleId });
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization().RequirePermission(Permissions.KnowledgeArticlesCreate);

        return app;
    }
}
