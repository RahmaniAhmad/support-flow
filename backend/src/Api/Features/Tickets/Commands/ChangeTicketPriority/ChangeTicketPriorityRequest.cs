using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Commands.ChangeTicketPriority;

public sealed record ChangeTicketPriorityRequest(
    TicketPriority Priority);
