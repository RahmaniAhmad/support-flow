using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.GetUnassignedTickets;

public static class GetUnassignedTicketsEndpoint
{
    public static IEndpointRouteBuilder MapGetUnassignedTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/unassigned",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var tickets = await sender.Send(
                    new GetUnassignedTicketsQuery(),
                    cancellationToken);

                return Results.Ok(tickets);
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsUnassign);

        return app;
    }
}
