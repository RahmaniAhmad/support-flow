using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;


namespace Api.Features.AI.GetTicketSuggestedArticles;


public static class GetTicketSuggestedArticlesEndpoint
{

    public static IEndpointRouteBuilder MapGetTicketSuggestedArticles(
        this IEndpointRouteBuilder endpoints)
    {

        endpoints.MapGet(
            "/tickets/{ticketId:guid}/suggested-articles",
            async (
                Guid ticketId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {

                var result =
                    await sender.Send(
                        new GetTicketSuggestedArticlesQuery(ticketId),
                        cancellationToken);


                return Results.Ok(result);

            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.AiTicketSuggestions);

        return endpoints;
    }

}