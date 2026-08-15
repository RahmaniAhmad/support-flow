namespace Api.Features.Authentication.ResetPassword;

public sealed record ResetPasswordRequest(
    string Token,
    string Password);