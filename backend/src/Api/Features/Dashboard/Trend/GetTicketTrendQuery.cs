using MediatR;

namespace Api.Features.Dashboard.Trend;


public sealed record GetTicketTrendQuery(
    Guid? CompanyId,
    DateOnly From,
    DateOnly To
)
: IRequest<IReadOnlyList<TicketTrendResponse>>;