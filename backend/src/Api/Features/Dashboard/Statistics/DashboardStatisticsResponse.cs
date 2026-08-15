namespace Api.Features.Dashboard.Statistics;

public sealed record DashboardStatisticsResponse(
    int TotalTickets,
    int OpenTickets,
    int AssignedTickets,
    int InProgressTickets,
    int PendingTickets,
    int ResolvedTickets,
    int ReopenedTickets,
    int ClosedTickets,
    int UnassignedTickets
);