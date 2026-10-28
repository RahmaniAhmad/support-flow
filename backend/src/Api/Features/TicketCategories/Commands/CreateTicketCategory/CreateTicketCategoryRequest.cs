namespace Api.Features.TicketCategories.Commands.CreateTicketCategory;

public sealed record CreateTicketCategoryRequest(
    string Name,
    string? Description);
