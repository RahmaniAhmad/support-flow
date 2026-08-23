namespace Api.Errors.ErrorCodes;

public static class UserErrorCodes
{
    public const string UserNotFound =
        "USER_NOT_FOUND";

    public const string CannotChangeStatus =
        "USER_STATUS_CHANGE_FORBIDDEN";

    public const string EmailAlreadyExists =
        "USER_EMAIL_ALREADY_EXISTS";

    public const string CannotCreateSuperAdmin =
        "USER_CANNOT_CREATE_SUPER_ADMIN";

    public const string AdminCannotCreateThisRole =
        "USER_ROLE_CREATION_FORBIDDEN";

    public const string CannotCreateUsers =
        "USER_CREATION_FORBIDDEN";

    public const string CannotResetPassword =
        "USER_PASSWORD_RESET_FORBIDDEN";
}