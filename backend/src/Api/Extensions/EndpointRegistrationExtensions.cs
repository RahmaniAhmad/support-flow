using Api.Features.AI;
using Api.Features.Authentication;
using Api.Features.Dashboard;
using Api.Features.KnowledgeBase;
using Api.Features.Notifications;
using Api.Features.TicketCategories;
using Api.Features.Tickets;
using Api.Features.Users;

namespace Api.Extensions;

public static class EndpointRegistrationExtensions
{
    public static WebApplication MapApplicationEndpoints(
        this WebApplication app)
    {
        app.MapAuthenticationEndpoints();

        app.MapUserEndpoints();

        app.MapDashboardEndpoints();

        app.MapTicketCategoryEndpoints();

        app.MapTicketEndpoints();

        app.MapKnowledgeBaseEndpoints();

        app.MapAIEndpoints();

        app.MapNotificationEndpoints();

        return app;
    }
}