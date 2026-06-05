using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.GetArticles
{
    public static class GetArticlesEndpoint
    {
        public static IEndpointRouteBuilder MapGetArticles(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/knowledge-articles",
                async (
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var articles = await db.KnowledgeArticles
                        .Where(x =>
                            x.CompanyId ==
                            currentUser.CompanyId)
                        .OrderByDescending(x =>
                            x.CreatedAtUtc)
                        .ToListAsync(
                            cancellationToken);

                    return Results.Ok(articles);
                })
                .RequireAuthorization();

            return app;
        }
    }
}