namespace Api.Errors.ErrorCodes;

public static class TicketErrorCodes
{
    public const string TicketNotFound =
        "TICKET_NOT_FOUND";

    public const string AssignedUserNotFound =
        "ASSIGNED_USER_NOT_FOUND";

    public const string CannotStartProgress =
        "CANNOT_START_PROGRESS";

    public const string CannotMoveToPending =
        "CANNOT_MOVE_TO_PENDING";

    public const string CannotAssignTicket =
        "CANNOT_ASSIGN_TICKET";

    public const string CannotResolveTicket =
        "CANNOT_RESOLVE_TICKET";

    public const string CannotCloseTicket =
        "CANNOT_CLOSE_TICKET";

    public const string CannotReopenTicket =
        "CANNOT_REOPEN_TICKET";

    public const string CannotComment =
        "CANNOT_COMMENT";
}