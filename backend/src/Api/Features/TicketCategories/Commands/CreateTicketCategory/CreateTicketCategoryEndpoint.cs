using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Commands.CreateTicketCategory;

public static class CreateTicketCategoryEndpoint
{
    public static IEndpointRouteBuilder MapCreateTicketCategory(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/ticket-categories/",
                async (
                    CreateTicketCategoryRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var categoryId = await sender.Send(
                        new CreateTicketCategoryCommand(
                            request.Name,
                            request.Description),
                        cancellationToken);

                    return Results.Created(
                        $"/ticket-categories/{categoryId}",
                        new { Id = categoryId });
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Create a ticket category");

        return endpoints;
    }
}
