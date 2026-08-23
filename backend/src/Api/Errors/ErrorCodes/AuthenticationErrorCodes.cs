namespace Api.Errors.ErrorCodes;

public static class AuthenticationErrorCodes
{
    public const string InvalidCredentials =
        "INVALID_CREDENTIALS";

    public const string EmailAlreadyRegistered =
        "EMAIL_ALREADY_REGISTERED";

    public const string InvalidPasswordResetToken =
        "INVALID_PASSWORD_RESET_TOKEN";

    public const string PasswordResetUnavailable =
        "PASSWORD_RESET_UNAVAILABLE";
}