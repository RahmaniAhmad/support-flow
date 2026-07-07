namespace Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public TimeSpan AccessTokenLifetime { get; init; } =
    TimeSpan.FromMinutes(15);

    public TimeSpan RefreshTokenLifetime { get; init; } =
        TimeSpan.FromDays(30);
}
