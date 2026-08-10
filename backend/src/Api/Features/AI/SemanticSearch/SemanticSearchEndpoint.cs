using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.AI.SemanticSearch;

public static class SemanticSearchEndpoint
{
    public static IEndpointRouteBuilder MapSemanticSearch(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/ai/semantic-search",
            async (
                SemanticSearchRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new SemanticSearchQuery(
                    request.Query,
                    request.Limit);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.AiSemanticSearch);

        return endpoints;
    }
}