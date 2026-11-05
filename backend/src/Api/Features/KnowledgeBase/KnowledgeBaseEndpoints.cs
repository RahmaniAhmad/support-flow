
using Api.Features.KnowledgeBase.Commands.CreateArticle;
using Api.Features.KnowledgeBase.Commands.DeleteArticle;
using Api.Features.KnowledgeBase.Commands.UpdateArticle;
using Api.Features.KnowledgeBase.Queries.GetArticle;
using Api.Features.KnowledgeBase.Queries.GetArticles;
using Api.Features.KnowledgeBase.Queries.SearchArticles;

namespace Api.Features.KnowledgeBase;

public static class KnowledgeBaseEndpoints
{
    public static IEndpointRouteBuilder MapKnowledgeBaseEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateArticle();
        endpoints.MapGetArticles();
        endpoints.MapGetArticle();
        endpoints.MapUpdateArticle();
        endpoints.MapDeleteArticle();
        endpoints.MapSearchArticles();

        return endpoints;
    }
}
