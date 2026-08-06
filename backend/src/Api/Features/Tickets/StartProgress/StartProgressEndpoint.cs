using Api.Filters;
using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.StartProgress;

public static class StartTicketProgressEndpoint
{
    public static IEndpointRouteBuilder MapStartProgress(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/start-progress",
            async (
                Guid ticketId,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new StartProgressCommand(ticketId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization();

        return app;
    }
}