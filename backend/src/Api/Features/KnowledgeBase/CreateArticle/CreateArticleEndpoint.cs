using Infrastructure.Persistence;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.KnowledgeBase.CreateArticle
{
    public static class CreateArticleEndpoint
    {
        public static IEndpointRouteBuilder MapCreateArticle(
            this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/knowledge-articles",
                async (
                    CreateArticleRequest request,
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var article = new KnowledgeArticle
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = currentUser.CompanyId,
                        Title = request.Title,
                        Content = request.Content,
                        CreatedAtUtc = DateTime.UtcNow
                    };

                    db.KnowledgeArticles.Add(article);

                    await db.SaveChangesAsync(
                        cancellationToken);

                    return Results.Created(
                        $"/knowledge-articles/{article.Id}",
                        article);
                })
                .RequireAuthorization();

            return app;
        }
    }
}