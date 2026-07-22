using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Tickets.ResolveTicket;

public static class ResolveTicketEndpoint
{
    public static IEndpointRouteBuilder MapResolveTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/resolve",
            async (
                Guid ticketId,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ResolveTicketCommand(ticketId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsResolve);

        return app;
    }
}