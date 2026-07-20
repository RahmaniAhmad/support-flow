using MediatR;
using Shared.Authentication;

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
        ICurrentUser currentUser,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetTicketsQuery(
            currentUser.CompanyId,
            request.Page,
            request.PageSize,
            request.Search,
            request.Status,
            request.AssignedToUserId,
            request.SortBy,
            request.Descending ?? true);

        var tickets = await sender.Send(query, cancellationToken);

        return Results.Ok(tickets);
    }
}