using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Commands.RemoveTicketCategory;

public static class RemoveTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapRemoveTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/tickets/{ticketId:guid}/category",
                async (
                    Guid ticketId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new RemoveTicketCategoryCommand(ticketId),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .RequirePermission(Permissions.TicketsAssign)
            .WithTags("Tickets")
            .WithSummary("Remove a ticket category");

        return endpoints;
    }
}
