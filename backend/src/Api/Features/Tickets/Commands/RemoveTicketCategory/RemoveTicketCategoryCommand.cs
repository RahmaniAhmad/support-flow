using MediatR;

namespace Api.Features.Tickets.Commands.RemoveTicketCategory;

public sealed record RemoveTicketCategoryCommand(Guid TicketId) : IRequest;
