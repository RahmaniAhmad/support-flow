namespace Api.Features.Authentication.Refresh;

public sealed record RefreshResponse(
    string AccessToken,
    string RefreshToken);
