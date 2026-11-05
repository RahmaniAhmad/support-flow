using Api.Features.TicketCategories.Commands.ActivateTicketCategory;
using Api.Features.TicketCategories.Commands.CreateTicketCategory;
using Api.Features.TicketCategories.Commands.DeactivateTicketCategory;
using Api.Features.TicketCategories.Commands.UpdateTicketCategory;
using Api.Features.TicketCategories.Queries.GetActiveTicketCategories;
using Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;
using Api.Features.TicketCategories.Queries.GetTicketCategories;
using Api.Features.TicketCategories.Queries.GetTicketCategoryById;

namespace Api.Features.TicketCategories;

public static class TicketCategoryEndpoints
{
    public static IEndpointRouteBuilder MapTicketCategoryEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetTicketCategories();
        endpoints.MapGetActiveTicketCategories();
        endpoints.MapGetActiveTicketCategoriesForTicket();
        endpoints.MapGetTicketCategoryById();
        endpoints.MapCreateTicketCategory();
        endpoints.MapUpdateTicketCategory();
        endpoints.MapActivateTicketCategory();
        endpoints.MapDeactivateTicketCategory();

        return endpoints;
    }
}
