using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.CloseTicket;

public record CloseTicketCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
  {
        $"tickets:company:{CompanyId}",
        $"tickets:company:{CompanyId}:ticket:{TicketId}",
        $"dashboard:{CompanyId}"
    };
}
