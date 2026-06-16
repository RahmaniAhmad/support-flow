using MediatR;

namespace Api.Features.Tickets.GetMyTickets;

public static class GetMyTicketsEndpoint
{
    public static IEndpointRouteBuilder MapGetMyTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/my",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetMyTicketsQuery(),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization();

        return app;
    }
}
