using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.StartTicketProgress;

public record StartTicketProgressCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
    {
        $"tickets:company:{CompanyId}",
        $"dashboard:{CompanyId}"
    };
}

