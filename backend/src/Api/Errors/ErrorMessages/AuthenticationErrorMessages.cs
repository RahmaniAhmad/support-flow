namespace Api.Errors.ErrorMessages;

public static class AuthenticationErrorMessages
{
    public const string InvalidCredentials =
        "Invalid email or password.";

    public const string EmailAlreadyRegistered =
        "This email address is already registered.";

    public const string InvalidPasswordResetToken =
        "The password reset link is invalid or has expired.";

    public const string PasswordResetUnavailable =
        "The password reset request cannot be completed.";
}