using MediatR;

namespace Api.Features.TicketCategories.Commands.DeactivateTicketCategory;

public sealed record DeactivateTicketCategoryCommand(Guid CategoryId) : IRequest;
