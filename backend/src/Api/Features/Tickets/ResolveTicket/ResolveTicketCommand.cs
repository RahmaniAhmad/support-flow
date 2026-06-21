using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.ResolveTicket;

public record ResolveTicketCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
{
        $"tickets:company:{CompanyId}",
        $"dashboard:{CompanyId}"
    };
}

