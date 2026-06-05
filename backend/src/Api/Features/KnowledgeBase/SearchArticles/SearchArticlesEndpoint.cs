using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.SearchArticles
{
    public static class SearchArticlesEndpoint
    {
        public static IEndpointRouteBuilder MapSearchArticles(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/knowledge-articles/search",
                async (
                    string query,
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(query))
                    {
                        return Results.BadRequest("Query cannot be empty.");
                    }

                    // PostgreSQL full-text search
                    var articles = await db.KnowledgeArticles
                        .Where(x => x.CompanyId == currentUser.CompanyId &&
                                    (EF.Functions.ILike(x.Title, $"%{query}%") ||
                                     EF.Functions.ILike(x.Content, $"%{query}%")))
                        .OrderByDescending(x => x.CreatedAtUtc)
                        .ToListAsync(cancellationToken);

                    return Results.Ok(articles);
                })
                .RequireAuthorization();

            return app;
        }
    }
}