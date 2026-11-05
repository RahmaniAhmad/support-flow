using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Commands.UpdateTicketCategory;

public static class UpdateTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapUpdateTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/ticket-categories/{categoryId:guid}",
                async (
                    Guid categoryId,
                    UpdateTicketCategoryRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new UpdateTicketCategoryCommand(
                            categoryId,
                            request.Name,
                            request.Description),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Update a ticket category");

        return endpoints;
    }
}
