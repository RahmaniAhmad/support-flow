using MediatR;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Commands.ChangeTicketPriority;

public sealed record ChangeTicketPriorityCommand(
    Guid TicketId,
    TicketPriority Priority) : IRequest;
