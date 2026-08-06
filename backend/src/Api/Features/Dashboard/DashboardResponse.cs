namespace Api.Features.Dashboard;

public sealed record DashboardResponse(
    int OpenTickets,
    int AssignedTickets,
    int InProgressTickets,
    int PendingTickets,
    int ResolvedTickets,
    int ReopenedTickets,
    int ClosedTickets,
    int UnassignedTickets);
