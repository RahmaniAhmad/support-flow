using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Tickets.ReopenTicket;

public static class ReopenTicketEndpoint
{
    public static IEndpointRouteBuilder MapReopenTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/reopen",
            async (
                Guid ticketId,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ReopenTicketCommand(ticketId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsReopen);

        return app;
    }
}
