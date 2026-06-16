using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.AssignTicket;

public static class AssignTicketEndpoint
{
    public static IEndpointRouteBuilder MapAssignTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/tickets/{ticketId:guid}/assign",
            async (
                Guid ticketId,
                AssignTicketRequest request,
                SupportFlowDbContext db,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new AssignTicketCommand(
                 ticketId,
                 currentUser.CompanyId,
                 request.AssignedToUserId);

                await sender.Send(command, cancellationToken);

                return Results.Ok();
            })
            .RequireAuthorization();

        return app;
    }
}
