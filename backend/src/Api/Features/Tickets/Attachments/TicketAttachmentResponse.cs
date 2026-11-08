namespace Api.Features.Tickets.Attachments;

public sealed record TicketAttachmentResponse(
    Guid Id,
    string FileName,
    string ContentType,
    long FileSize,
    Guid UploadedByUserId,
    DateTime CreatedAtUtc);
