using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Queries.GetTicketCategoryById;

public static class GetTicketCategoryByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetTicketCategoryById(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/ticket-categories/{categoryId:guid}",
                async (
                    Guid categoryId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetTicketCategoryByIdQuery(categoryId),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesView)
            .WithSummary("Get a ticket category");

        return endpoints;
    }
}
