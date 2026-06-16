namespace Api.Features.Tickets.GetComments;

public sealed record GetTicketCommentsResponse(
    Guid Id,
    string Content,
    DateTime CreatedAtUtc);