using MediatR;
using Shared.Authentication;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTicketsByStatus;

public static class GetTicketsByStatusEndpoint
{
    public static IEndpointRouteBuilder MapGetTicketsByStatus(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/status/{status}",
            async (
                TicketStatus status,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var tickets = await sender.Send(
                    new GetTicketsByStatusQuery(currentUser.CompanyId, status),
                    cancellationToken);

                return Results.Ok(tickets);
            })
            .RequireAuthorization();

        return app;
    }
}
