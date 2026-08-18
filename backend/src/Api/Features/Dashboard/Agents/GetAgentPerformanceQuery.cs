using MediatR;

namespace Api.Features.Dashboard.Agents;

public sealed record GetAgentPerformanceQuery(
    Guid? CompanyId
) : IRequest<IReadOnlyList<AgentPerformanceResponse>>;