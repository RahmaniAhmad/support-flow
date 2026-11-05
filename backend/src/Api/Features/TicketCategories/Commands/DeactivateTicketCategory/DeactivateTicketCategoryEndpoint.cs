using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Commands.DeactivateTicketCategory;

public static class DeactivateTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapDeactivateTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/ticket-categories/{categoryId:guid}/deactivate",
                async (
                    Guid categoryId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new DeactivateTicketCategoryCommand(categoryId),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Deactivate a ticket category");

        return endpoints;
    }
}
