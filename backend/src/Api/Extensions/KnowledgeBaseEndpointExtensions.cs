using Api.Features.KnowledgeBase.Commands.CreateArticle;
using Api.Features.KnowledgeBase.Commands.DeleteArticle;
using Api.Features.KnowledgeBase.Queries.GetArticle;
using Api.Features.KnowledgeBase.Queries.GetArticles;
using Api.Features.KnowledgeBase.Queries.SearchArticles;
using Api.Features.KnowledgeBase.Commands.UpdateArticle;

namespace Api.Extensions;

public static class KnowledgeBaseEndpointExtensions
{
    public static WebApplication MapKnowledgeBaseEndpoints(
        this WebApplication app)
    {
        app.MapCreateArticle();
        app.MapGetArticles();
        app.MapGetArticle();
        app.MapUpdateArticle();
        app.MapDeleteArticle();
        app.MapSearchArticles();

        return app;
    }
}
