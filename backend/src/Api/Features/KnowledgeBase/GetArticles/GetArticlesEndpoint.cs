using MediatR;
using Api.Authorization;
using Shared.Domain.Users;

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
            .RequireAuthorization().RequirePermission(Permissions.KnowledgeArticlesView);

        return app;
    }
}
