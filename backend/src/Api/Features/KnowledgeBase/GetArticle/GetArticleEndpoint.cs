using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.GetArticle
{

    public static class GetArticleEndpoint
    {
        public static IEndpointRouteBuilder MapGetArticle(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
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

                    return article is null
                        ? Results.NotFound()
                        : Results.Ok(article);
                })
                .RequireAuthorization();

            return app;
        }
    }
}