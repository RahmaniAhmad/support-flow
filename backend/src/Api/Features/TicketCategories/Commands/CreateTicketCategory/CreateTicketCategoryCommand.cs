using MediatR;

namespace Api.Features.TicketCategories.Commands.CreateTicketCategory;

public sealed record CreateTicketCategoryCommand(
    string Name,
    string? Description) : IRequest<Guid>;
