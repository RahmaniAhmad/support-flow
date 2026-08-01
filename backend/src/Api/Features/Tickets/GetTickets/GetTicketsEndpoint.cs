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
        [AsParameters] GetTicketsRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketsQuery(
            request.Page,
            request.PageSize,
            request.Search,
            request.Status,
            request.View,
            request.SortBy,
            request.Descending ?? true);

        var tickets = await sender.Send(query, cancellationToken);

        return Results.Ok(tickets);
    }
}