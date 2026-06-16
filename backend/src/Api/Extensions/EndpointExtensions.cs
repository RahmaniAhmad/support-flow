using Api.Features.Authentication.Login;
using Api.Features.Authentication.RegisterCompany;
using Api.Features.Authentication.Me;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapRegisterCompany();

        app.MapLogin();

        app.MapMe();

        return app;
    }
}