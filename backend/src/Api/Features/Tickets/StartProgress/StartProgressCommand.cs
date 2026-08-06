using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.StartProgress;

public record StartProgressCommand(
    Guid TicketId) : IRequest;

