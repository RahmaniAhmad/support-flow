using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.CloseTicket;

public static class CloseTicketEndpoint
{
    public static IEndpointRouteBuilder MapCloseTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/close",
            async (
                Guid ticketId,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CloseTicketCommand(
                    ticketId,
                    currentUser.CompanyId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .RequireAuthorization();

        return app;
    }
}