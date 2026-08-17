using MediatR;

namespace Api.Features.Dashboard.Unassigned;

public sealed record GetUnassignedTicketsQuery(
    Guid? CompanyId,
    int Limit = 5)
    : IRequest<IReadOnlyList<UnassignedTicketResponse>>;