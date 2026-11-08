using MediatR;

namespace Api.Features.Tickets.Attachments.DownloadTicketAttachment;

public sealed record DownloadTicketAttachmentQuery(
    Guid Id) : IRequest<DownloadTicketAttachmentResponse?>;
