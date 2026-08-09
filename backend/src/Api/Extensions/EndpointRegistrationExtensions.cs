namespace Api.Extensions;

public static class EndpointRegistrationExtensions
{
    public static WebApplication MapApplicationEndpoints(
        this WebApplication app)
    {
        app.MapAuthenticationEndpoints();

        app.MapUserEndpoints();

        app.MapDashboardEndpoints();


        app.MapTicketEndpoints();

        app.MapKnowledgeBaseEndpoints();

        app.MapAIEndpoints();

        return app;
    }
}