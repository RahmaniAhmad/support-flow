using Api.Features.AI.Chat;
using Api.Features.AI.GetTicketSuggestedArticles;
using Api.Features.AI.SemanticSearch;

namespace Api.Features.AI;

public static class AIEndpoints
{
    public static IEndpointRouteBuilder MapAIEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapSemanticSearch();
        endpoints.MapGetTicketSuggestedArticles();
        endpoints.MapChat();

        return endpoints;
    }
}
