using MediatR;

namespace Api.Features.Tickets.Commands.AssignTicketCategory;

public sealed record AssignTicketCategoryCommand(
    Guid TicketId,
    Guid CategoryId) : IRequest;
