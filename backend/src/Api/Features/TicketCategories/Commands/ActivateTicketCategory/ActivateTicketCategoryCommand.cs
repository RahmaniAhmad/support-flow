using MediatR;

namespace Api.Features.TicketCategories.Commands.ActivateTicketCategory;

public sealed record ActivateTicketCategoryCommand(Guid CategoryId) : IRequest;
