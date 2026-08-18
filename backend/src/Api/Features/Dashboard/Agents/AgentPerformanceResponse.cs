namespace Api.Features.Dashboard.Agents;

public sealed record AgentPerformanceResponse(
    Guid UserId,
    string Name,
    int AssignedTickets,
    int ResolvedTickets
);