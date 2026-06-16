using MediatR;

namespace Api.Features.Tickets.StartTicketProgress;

public record StartTicketProgressCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest;
