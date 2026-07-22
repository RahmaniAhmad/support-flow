using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.StartTicketProgress;

public record StartTicketProgressCommand(
    Guid TicketId) : IRequest;

