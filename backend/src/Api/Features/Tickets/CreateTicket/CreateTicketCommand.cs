using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.CreateTicket;

public record CreateTicketCommand(
    string Subject,
    string Description) : IRequest<Guid>;
