using MediatR;

namespace Api.Features.Tickets.Attachments.DeleteTicketAttachment;

public sealed record DeleteTicketAttachmentCommand(
    Guid Id) : IRequest;
