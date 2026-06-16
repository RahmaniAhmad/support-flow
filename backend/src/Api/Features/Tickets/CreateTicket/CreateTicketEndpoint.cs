using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Tickets.CreateTicket;


public static class CreateTicketEndpoint
{
    public static IEndpointRouteBuilder MapCreateTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets",
            async (
                CreateTicketRequest request,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {

                var command = new CreateTicketCommand(
                    currentUser.CompanyId,
                    currentUser.UserId,
                    request.Subject,
                    request.Description);

                var ticketId = await sender.Send(command, cancellationToken);

                return Results.Created($"/tickets/{ticketId}", new { Id = ticketId });

            })
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsCreate);

        return app;
    }
}

