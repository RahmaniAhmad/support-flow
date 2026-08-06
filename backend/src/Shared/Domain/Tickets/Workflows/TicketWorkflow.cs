namespace Shared.Domain.Tickets.Workflows;

public static class TicketWorkflow
{
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions =
        new()
        {
            [TicketStatus.Open] =
            [
                TicketStatus.Assigned,
                TicketStatus.Closed
            ],

            [TicketStatus.Assigned] =
            [
                TicketStatus.InProgress,
                TicketStatus.Closed
            ],

            [TicketStatus.InProgress] =
            [
                TicketStatus.Pending,
                TicketStatus.Resolved,
                TicketStatus.Closed
            ],

            [TicketStatus.Pending] =
            [
                TicketStatus.InProgress,
                TicketStatus.Resolved,
                TicketStatus.Closed
            ],

            [TicketStatus.Resolved] =
            [
                TicketStatus.Closed,
                TicketStatus.Reopened
            ],

            [TicketStatus.Closed] =
            [
                TicketStatus.Reopened
            ],

            [TicketStatus.Reopened] =
            [
                TicketStatus.Assigned,
                TicketStatus.Closed
            ]
        };

    public static bool CanTransition(
        TicketStatus currentStatus,
        TicketStatus newStatus)
    {
        return AllowedTransitions.TryGetValue(
            currentStatus,
            out var allowedStatuses)
            &&
            allowedStatuses.Contains(newStatus);
    }
}