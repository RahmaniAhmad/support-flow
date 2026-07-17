namespace Api.Features.Authentication;

public static class AuthenticationCookieOptions
{
    public static CookieOptions AccessToken(HttpContext context, TimeSpan lifetime)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.Add(lifetime),
            Path = "/"
        };
    }

    public static CookieOptions RefreshToken(HttpContext context, TimeSpan lifetime)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.Add(lifetime),
            Path = "/"
        };
    }

    public static CookieOptions Csrf(HttpContext context, TimeSpan lifetime)
    {
        return new CookieOptions
        {
            HttpOnly = false, // JavaScript must read it
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.Add(lifetime),
            Path = "/"
        };
    }
}