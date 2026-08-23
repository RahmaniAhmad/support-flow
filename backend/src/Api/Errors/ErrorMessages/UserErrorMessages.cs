namespace Api.Errors.ErrorMessages;

public static class UserErrorMessages
{
    public const string UserNotFound =
        "User was not found.";

    public const string CannotChangeStatus =
        "You are not allowed to change this user's status.";

    public const string EmailAlreadyExists =
        "A user with this email address already exists.";

    public const string CannotCreateSuperAdmin =
        "You are not allowed to create a SuperAdmin user.";

    public const string AdminCannotCreateThisRole =
        "An Admin can only create Agent or Customer users.";

    public const string CannotCreateUsers =
        "You are not allowed to create users.";

    public const string CannotResetPassword =
        "You are not allowed to reset this user's password.";
}