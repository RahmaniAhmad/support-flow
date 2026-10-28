using Api.Features.TicketCategories.Commands.AssignTicketCategory;
using Api.Features.TicketCategories.Commands.ActivateTicketCategory;
using Api.Features.TicketCategories.Commands.CreateTicketCategory;
using Api.Features.TicketCategories.Commands.DeactivateTicketCategory;
using Api.Features.TicketCategories.Commands.RemoveTicketCategory;
using Api.Features.TicketCategories.Commands.UpdateTicketCategory;
using Api.Features.TicketCategories.Queries.GetActiveTicketCategories;
using Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;
using Api.Features.TicketCategories.Queries.GetTicketCategories;
using Api.Features.TicketCategories.Queries.GetTicketCategoryById;
using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.TicketCategories;

public static class TicketCategoryEndpoints
{
    public static IEndpointRouteBuilder MapTicketCategoryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var categories = endpoints.MapGroup("/ticket-categories")
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .WithTags("Ticket Categories");

        categories.MapGet(
                "/",
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
            .RequirePermission(Permissions.TicketCategoriesView)
            .WithSummary("List tenant ticket categories");

        categories.MapGet(
                "/active",
                async (
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        new GetActiveTicketCategoriesQuery(),
                        cancellationToken);

                    return Results.Ok(result);
                })
            .RequirePermission(Permissions.TicketsCreate)
            .WithSummary("List active categories for ticket creation");

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

        categories.MapGet(
                "/{categoryId:guid}",
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
            .RequirePermission(Permissions.TicketCategoriesView)
            .WithSummary("Get a ticket category");

        categories.MapPost(
                "/",
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
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Create a ticket category");

        categories.MapPut(
                "/{categoryId:guid}",
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
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Update a ticket category");

        categories.MapPut(
                "/{categoryId:guid}/activate",
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
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Activate a ticket category");

        categories.MapPut(
                "/{categoryId:guid}/deactivate",
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
            .RequirePermission(Permissions.TicketCategoriesManage)
            .WithSummary("Deactivate a ticket category");

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
