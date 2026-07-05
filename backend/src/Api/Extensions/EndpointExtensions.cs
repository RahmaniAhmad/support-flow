using Api.Features.Authentication.Login;
using Api.Features.Authentication.RegisterCompany;
using Api.Features.Authentication.Me;
using Api.Features.Authentication.Logout;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(
        this WebApplication app)
    {
        app.MapRegisterCompany();

        app.MapLogin();

        app.MapLogout();

        app.MapMe();

        return app;
    }
}