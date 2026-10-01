namespace Api.Features.Tickets.Queries.GetComments;

public sealed record GetTicketCommentsResponse(
    Guid Id,
    string Content,
    Guid AuthorUserId,
    string AuthorName,
    string AuthorEmail,
    DateTime CreatedAtUtc);