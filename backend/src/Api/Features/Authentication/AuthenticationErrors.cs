namespace Api.Features.Authentication;

internal static class AuthenticationErrors
{
    public const string InvalidCredentialsMessage =
        "Invalid email or password.";

    public const string InvalidCredentialsCode =
        "INVALID_CREDENTIALS";

    public const string EmailAlreadyRegisteredMessage =
        "This email address is already registered.";

    public const string EmailAlreadyRegisteredCode =
        "EMAIL_ALREADY_REGISTERED";

    public const string InvalidPasswordResetTokenMessage =
        "The password reset link is invalid or has expired.";

    public const string InvalidPasswordResetTokenCode =
        "INVALID_PASSWORD_RESET_TOKEN";

    public const string PasswordResetUnavailableMessage =
        "The password reset request cannot be completed.";

    public const string PasswordResetUnavailableCode =
        "PASSWORD_RESET_UNAVAILABLE";
}