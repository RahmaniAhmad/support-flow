

using Api.Features.Dashboard.Activity;
using Api.Features.Dashboard.Agents;
using Api.Features.Dashboard.Statistics;
using Api.Features.Dashboard.Trend;
using Api.Features.Dashboard.Unassigned;

namespace Api.Features.Dashboard;

public static class DashboardEndpoints
{
    public static IEndpointRouteBuilder MapDashboardEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDashboardStatistics();
        endpoints.MapTicketTrend();
        endpoints.MapAgentPerformance();
        endpoints.MapRecentActivities();
        endpoints.MapUnassignedTickets();

        return endpoints;
    }
}
