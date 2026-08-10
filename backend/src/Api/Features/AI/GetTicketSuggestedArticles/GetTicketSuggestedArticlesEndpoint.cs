using MediatR;


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
            .RequireAuthorization();


        return endpoints;
    }

}