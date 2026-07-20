namespace Api.Extensions;

public static class EndpointRegistrationExtensions
{
    public static WebApplication MapApplicationEndpoints(
        this WebApplication app)
    {
        app.MapAuthenticationEndpoints();

        app.MapDashboardEndpoints();

        app.MapTicketEndpoints();

        app.MapKnowledgeBaseEndpoints();


        return app;
    }
}