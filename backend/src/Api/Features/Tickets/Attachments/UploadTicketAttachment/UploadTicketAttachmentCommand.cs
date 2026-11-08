using MediatR;

namespace Api.Features.Tickets.Attachments.UploadTicketAttachment;

public sealed record UploadTicketAttachmentCommand(
    Guid TicketId,
    IFormFile File) : IRequest<TicketAttachmentResponse>;
