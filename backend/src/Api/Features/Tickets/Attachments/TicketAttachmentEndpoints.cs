using Api.Features.Tickets.Attachments.DeleteTicketAttachment;
using Api.Features.Tickets.Attachments.DownloadTicketAttachment;
using Api.Features.Tickets.Attachments.GetTicketAttachments;
using Api.Features.Tickets.Attachments.UploadTicketAttachment;

namespace Api.Features.Tickets.Attachments;

public static class TicketAttachmentEndpoints
{
    public static IEndpointRouteBuilder MapTicketAttachmentEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapUploadTicketAttachment();
        endpoints.MapGetTicketAttachments();
        endpoints.MapDownloadTicketAttachment();
        endpoints.MapDeleteTicketAttachment();

        return endpoints;
    }
}
