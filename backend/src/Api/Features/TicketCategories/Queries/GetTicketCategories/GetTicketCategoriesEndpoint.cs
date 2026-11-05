using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public static class GetTicketCategoriesEndpoint
{
    public static IEndpointRouteBuilder MapGetTicketCategories(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/ticket-categories/",
                async (
                    [AsParameters] GetTicketCategoriesRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetTicketCategoriesQuery(
                            request.Page,
                            request.PageSize,
                            request.Search),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories")
            .RequirePermission(Permissions.TicketCategoriesView)
            .WithSummary("List tenant ticket categories");

        return endpoints;
    }
}
