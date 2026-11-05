namespace Api.Features.Users;

internal static class UserErrors
{
    public const string NotFoundMessage =
        "User was not found.";

    public const string NotFoundCode =
        "USER_NOT_FOUND";

    public const string CannotChangeStatusMessage =
        "You are not allowed to change this user's status.";

    public const string CannotChangeStatusCode =
        "USER_STATUS_CHANGE_FORBIDDEN";

    public const string EmailAlreadyExistsMessage =
        "A user with this email address already exists.";

    public const string EmailAlreadyExistsCode =
        "USER_EMAIL_ALREADY_EXISTS";

    public const string CannotCreateSuperAdminMessage =
        "You are not allowed to create a SuperAdmin user.";

    public const string CannotCreateSuperAdminCode =
        "USER_CANNOT_CREATE_SUPER_ADMIN";

    public const string AdminCannotCreateThisRoleMessage =
        "An Admin can only create Agent or Customer users.";

    public const string AdminCannotCreateThisRoleCode =
        "USER_ROLE_CREATION_FORBIDDEN";

    public const string CannotCreateUsersMessage =
        "You are not allowed to create users.";

    public const string CannotCreateUsersCode =
        "USER_CREATION_FORBIDDEN";

    public const string CannotResetPasswordMessage =
        "You are not allowed to reset this user's password.";

    public const string CannotResetPasswordCode =
        "USER_PASSWORD_RESET_FORBIDDEN";
}