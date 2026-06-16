namespace Api.Features.Dashboard;

public sealed record DashboardResponse(
    int OpenTickets,
    int InProgressTickets,
    int ResolvedTickets,
    int closedTickets,
    int UnassignedTickets);
