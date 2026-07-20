namespace Shared.Domain.Tickets.Workflows;

public static class TicketWorkflow
{
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions =
        new()
        {
            [TicketStatus.Open] =
            [
                TicketStatus.InProgress,
                TicketStatus.Closed
            ],

            [TicketStatus.InProgress] =
            [
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
                TicketStatus.InProgress,
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