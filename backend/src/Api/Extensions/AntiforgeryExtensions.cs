using Api.Features.Authentication;
using Microsoft.AspNetCore.Antiforgery;

namespace Api.Extensions;

public static class AntiforgeryExtensions
{
    public static void IssueXsrfToken(
        this HttpContext context,
        TimeSpan lifetime)
    {
        var antiforgery =
            context.RequestServices.GetRequiredService<IAntiforgery>();

        var tokens =
            antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append(
            "XSRF-TOKEN",
            tokens.RequestToken!,
            AuthenticationCookieOptions.Csrf(context, lifetime));
    }
}