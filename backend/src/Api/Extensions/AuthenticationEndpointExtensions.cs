using Api.Features.Authentication.Login;
using Api.Features.Authentication.RegisterCompany;
using Api.Features.Authentication.Me;
using Api.Features.Authentication.Logout;
using Api.Features.Authentication.Refresh;
using Api.Features.Authentication.Csrf;
using Api.Features.Authentication.ForgotPassword;
using Api.Features.Authentication.ResetPassword;

namespace Api.Extensions;

public static class EndpointExtensions
{
    public static WebApplication MapAuthenticationEndpoints(
        this WebApplication app)
    {
        app.MapRegisterCompany();
        app.MapLogin();
        app.MapLogout();
        app.MapForgotPassword();
        app.MapResetPassword();
        app.MapRefresh();
        app.MapCsrf();
        app.MapMe();

        return app;
    }
}