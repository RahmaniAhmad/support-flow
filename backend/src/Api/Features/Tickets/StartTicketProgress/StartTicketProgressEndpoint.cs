using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.StartTicketProgress;

public static class StartTicketProgressEndpoint
{
    public static IEndpointRouteBuilder MapStartTicketProgress(
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
                var command = new StartTicketProgressCommand(
                    ticketId,
                    currentUser.CompanyId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .RequireAuthorization();

        return app;
    }
}