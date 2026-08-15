namespace Api.Features.Dashboard.Trend;

public sealed record TicketTrendResponse(
    DateOnly Date,
    int Created,
    int Resolved
);