using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;

public static class GetActiveTicketCategoriesForTicketEndpoint
{
    public static IEndpointRouteBuilder MapGetActiveTicketCategoriesForTicket(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/tickets/{ticketId:guid}/ticket-categories/active",
                async (
                    Guid ticketId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetActiveTicketCategoriesForTicketQuery(ticketId),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsAssign)
            .WithSummary("List active categories available to a ticket");

        return endpoints;
    }
}
