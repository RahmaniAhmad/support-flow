using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.ReopenTicket;

public record ReopenTicketCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
 {
        $"tickets:company:{CompanyId}",
        $"dashboard:{CompanyId}"
    };
}
