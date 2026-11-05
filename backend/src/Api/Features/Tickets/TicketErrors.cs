namespace Api.Features.Tickets;

internal static class TicketErrors
{
    public const string NotFoundMessage =
        "Ticket was not found.";

    public const string NotFoundCode =
        "NOT_FOUND";

    public const string AssignedUserNotFoundMessage =
        "The user to assign the ticket to was not found.";

    public const string AssignedUserNotFoundCode =
        "ASSIGNED_USER_NOT_FOUND";

    public const string CannotStartProgressMessage =
        "You are not allowed to start progress on this ticket.";

    public const string CannotStartProgressCode =
        "CANNOT_START_PROGRESS";

    public const string CannotMoveToPendingMessage =
        "You are not allowed to move this ticket to pending.";

    public const string CannotMoveToPendingCode =
        "CANNOT_MOVE_TO_PENDING";

    public const string CannotAssignTicketMessage =
        "You are not allowed to assign this ticket.";

    public const string CannotAssignTicketCode =
        "CANNOT_ASSIGN_TICKET";

    public const string CannotResolveTicketMessage =
        "You are not allowed to resolve this ticket.";

    public const string CannotResolveTicketCode =
        "CANNOT_RESOLVE_TICKET";

    public const string CannotCloseTicketMessage =
        "You are not allowed to close this ticket.";

    public const string CannotCloseTicketCode =
        "CANNOT_CLOSE_TICKET";

    public const string CannotReopenTicketMessage =
        "You are not allowed to reopen this ticket.";

    public const string CannotReopenTicketCode =
        "CANNOT_REOPEN_TICKET";

    public const string CannotCommentMessage =
        "You are not allowed to comment on this ticket.";

    public const string CannotCommentCode =
        "CANNOT_COMMENT";

    public const string InvalidTicketTransitionMessage =
        "The ticket cannot be moved to the requested status.";

    public const string InvalidTicketTransitionCode =
        "INVALID_TICKET_TRANSITION";

    public const string NotAssignedMessage =
        "The ticket must be assigned to an agent before this action can be performed.";

    public const string NotAssignedCode =
        "NOT_ASSIGNED";

    public const string NotAssignedAgentMessage =
        "Only the assigned agent can perform this action.";

    public const string NotAssignedAgentCode =
        "NOT_ASSIGNED_AGENT";
}