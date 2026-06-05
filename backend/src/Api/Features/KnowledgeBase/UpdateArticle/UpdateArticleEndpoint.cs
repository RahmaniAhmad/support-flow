using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;

using Shared.Authentication;

namespace Api.Features.KnowledgeBase.UpdateArticle
{
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
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var article = await db.KnowledgeArticles
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id == id &&
                                x.CompanyId == currentUser.CompanyId,
                            cancellationToken);

                    if (article is null)
                    {
                        return Results.NotFound();
                    }

                    article.Title = request.Title;
                    article.Content = request.Content;
                    article.UpdatedAtUtc = DateTime.UtcNow;

                    await db.SaveChangesAsync(
                        cancellationToken);

                    return Results.Ok(article);
                })
                .RequireAuthorization();

            return app;
        }
    }
}