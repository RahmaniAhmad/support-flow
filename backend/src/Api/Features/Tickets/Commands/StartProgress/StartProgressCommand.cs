using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.Commands.StartProgress;

public record StartProgressCommand(
    Guid TicketId) : IRequest;

