using MediatR;

namespace Api.Features.TicketCategories.Commands.AssignTicketCategory;

public sealed record AssignTicketCategoryCommand(
    Guid TicketId,
    Guid CategoryId) : IRequest;
