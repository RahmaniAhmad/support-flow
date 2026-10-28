namespace Api.Features.TicketCategories.Commands.UpdateTicketCategory;

public sealed record UpdateTicketCategoryRequest(
    string Name,
    string? Description);
