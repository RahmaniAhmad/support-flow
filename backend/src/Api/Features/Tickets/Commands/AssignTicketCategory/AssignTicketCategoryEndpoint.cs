using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Commands.AssignTicketCategory;

public static class AssignTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapAssignTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/tickets/{ticketId:guid}/category",
                async (
                    Guid ticketId,
                    AssignTicketCategoryRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new AssignTicketCategoryCommand(
                            ticketId,
                            request.CategoryId),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .RequirePermission(Permissions.TicketsAssign)
            .WithTags("Tickets")
            .WithSummary("Assign a category to a ticket");

        return endpoints;
    }
}
