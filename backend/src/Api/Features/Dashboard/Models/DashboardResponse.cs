namespace Api.Features.Dashboard.Models
{
    public sealed record DashboardResponse(
        int OpenTickets,
        int InProgressTickets,
        int ResolvedTickets,
        int UnassignedTickets);
}