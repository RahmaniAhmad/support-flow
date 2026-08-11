using Api.Features.AI.GetTicketSuggestedArticles;
using Api.Features.AI.SemanticSearch;

namespace Api.Extensions;


public static class AIEndpointExtensions
{
    public static WebApplication MapAIEndpoints(
        this WebApplication app)
    {
        app.MapSemanticSearch();
        app.MapGetTicketSuggestedArticles();
        return app;
    }
}
