namespace Api.Features.Tickets.Attachments.DownloadTicketAttachment;

public sealed record DownloadTicketAttachmentResponse(
    Stream Content,
    string FileName,
    string ContentType);
