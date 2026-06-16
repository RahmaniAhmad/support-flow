using MediatR;

namespace Api.Features.Tickets.GetTickets;

public static class GetTicketsEndpoint
{
    public static IEndpointRouteBuilder MapGetTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets",
            GetTicketsAsync)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetTicketsAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var tickets = await sender.Send(
            new GetTicketsQuery(),
            cancellationToken);

        return Results.Ok(tickets);
    }
}