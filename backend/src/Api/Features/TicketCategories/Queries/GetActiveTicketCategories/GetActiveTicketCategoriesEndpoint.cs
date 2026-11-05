using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategories;

public static class GetActiveTicketCategoriesEndpoint
{
    public static IEndpointRouteBuilder MapGetActiveTicketCategories(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/ticket-categories/active",
                async (
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetActiveTicketCategoriesQuery(),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketsCreate)
            .WithSummary("List active categories for ticket creation");

        return endpoints;
    }
}
