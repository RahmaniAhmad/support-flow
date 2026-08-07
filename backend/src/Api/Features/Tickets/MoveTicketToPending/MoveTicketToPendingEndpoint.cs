using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Tickets.MoveTicketToPending;

public static class MoveTicketToPendingEndpoint
{
    public static IEndpointRouteBuilder MapMoveTicketToPending(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/move-to-pending",
            async (
                Guid ticketId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new MoveTicketToPendingCommand(ticketId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsMoveToPending);

        return app;
    }
}