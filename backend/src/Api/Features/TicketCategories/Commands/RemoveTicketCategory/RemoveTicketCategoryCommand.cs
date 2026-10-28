using MediatR;

namespace Api.Features.TicketCategories.Commands.RemoveTicketCategory;

public sealed record RemoveTicketCategoryCommand(Guid TicketId) : IRequest;
