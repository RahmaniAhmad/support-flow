namespace Api.Features.TicketCategories.Queries;

public sealed record TicketCategoryResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
