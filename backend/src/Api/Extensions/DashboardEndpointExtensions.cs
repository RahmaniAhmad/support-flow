using Api.Features.Dashboard;

namespace Api.Extensions;

public static class DashboardEndpointExtensions
{
    public static WebApplication MapDashboardEndpoints(
        this WebApplication app)
    {
        app.MapDashboard();

        return app;
    }
}