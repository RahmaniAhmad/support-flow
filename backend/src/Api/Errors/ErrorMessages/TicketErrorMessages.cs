namespace Api.Errors.ErrorMessages;

public static class TicketErrorMessages
{
    public const string TicketNotFound =
        "Ticket was not found.";

    public const string AssignedUserNotFound =
        "The user to assign the ticket to was not found.";

    public const string CannotStartProgress =
        "You are not allowed to start progress on this ticket.";

    public const string CannotMoveToPending =
        "You are not allowed to move this ticket to pending.";

    public const string CannotAssignTicket =
        "You are not allowed to assign this ticket.";

    public const string CannotResolveTicket =
        "You are not allowed to resolve this ticket.";

    public const string CannotCloseTicket =
        "You are not allowed to close this ticket.";

    public const string CannotReopenTicket =
        "You are not allowed to reopen this ticket.";

    public const string CannotComment =
        "You are not allowed to comment on this ticket.";
}