using Api.Features.Dashboard.Activity;
using Api.Features.Dashboard.Agents;
using Api.Features.Dashboard.Statistics;
using Api.Features.Dashboard.Trend;
using Api.Features.Dashboard.Unassigned;

namespace Api.Extensions;

public static class DashboardEndpointExtensions
{
    public static WebApplication MapDashboardEndpoints(
        this WebApplication app)
    {
        app.MapDashboardStatistics();
        app.MapTicketTrend();
        app.MapAgentPerformance();
        app.MapRecentActivities();
        app.MapUnassignedTickets();

        return app;
    }
}