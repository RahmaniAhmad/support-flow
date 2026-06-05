using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.DeleteArticle
{
    public static class DeleteArticleEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteArticle(
            this IEndpointRouteBuilder app)
        {
            app.MapDelete(
                "/knowledge-articles/{id:guid}",
                async (
                    Guid id,
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

                    db.KnowledgeArticles.Remove(article);

                    await db.SaveChangesAsync(
                        cancellationToken);

                    return Results.NoContent();
                })
                .RequireAuthorization();

            return app;
        }
    }
}