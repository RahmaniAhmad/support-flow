using Api.Features.KnowledgeBase.CreateArticle;
using Api.Features.KnowledgeBase.DeleteArticle;
using Api.Features.KnowledgeBase.GetArticle;
using Api.Features.KnowledgeBase.GetArticles;
using Api.Features.KnowledgeBase.SearchArticles;
using Api.Features.KnowledgeBase.UpdateArticle;

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
