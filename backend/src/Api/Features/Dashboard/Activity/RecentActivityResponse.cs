namespace Api.Features.Dashboard.Activity;

public sealed record RecentActivityResponse(
    string Message,
    DateTime CreatedAtUtc
);