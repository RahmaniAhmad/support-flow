namespace Api.Features.Dashboard.Unassigned;

public sealed record UnassignedTicketResponse(
    Guid Id,
    long TicketNumber,
    string Subject,
    string CreatedBy,
    DateTime CreatedAtUtc);