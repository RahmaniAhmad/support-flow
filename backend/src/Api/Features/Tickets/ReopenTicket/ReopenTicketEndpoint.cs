using MediatR;
using Shared.Authentication;

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
                var command = new ReopenTicketCommand(
                    ticketId,
                    currentUser.CompanyId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .RequireAuthorization();

        return app;
    }
}
