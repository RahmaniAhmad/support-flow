using MediatR;

namespace Api.Features.Tickets.Attachments.GetTicketAttachments;

public sealed record GetTicketAttachmentsQuery(
    Guid TicketId) : IRequest<IReadOnlyList<TicketAttachmentResponse>?>;
