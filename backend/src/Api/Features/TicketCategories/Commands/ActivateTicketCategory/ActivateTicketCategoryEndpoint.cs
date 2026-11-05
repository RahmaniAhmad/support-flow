using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Commands.ActivateTicketCategory;

public static class ActivateTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapActivateTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/ticket-categories/{categoryId:guid}/activate",
                async (
                    Guid categoryId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new ActivateTicketCategoryCommand(categoryId),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Activate a ticket category");

        return endpoints;
    }
}
