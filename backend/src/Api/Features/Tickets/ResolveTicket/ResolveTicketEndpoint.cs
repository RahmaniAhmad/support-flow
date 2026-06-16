using MediatR;
using Shared.Authentication;

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
                var command = new ResolveTicketCommand(
                    ticketId,
                    currentUser.CompanyId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .RequireAuthorization();

        return app;
    }
}