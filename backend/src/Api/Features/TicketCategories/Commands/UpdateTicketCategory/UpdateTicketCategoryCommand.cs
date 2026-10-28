using MediatR;

namespace Api.Features.TicketCategories.Commands.UpdateTicketCategory;

public sealed record UpdateTicketCategoryCommand(
    Guid CategoryId,
    string Name,
    string? Description) : IRequest;
