using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Attachments.DownloadTicketAttachment;

public static class DownloadTicketAttachmentEndpoint
{
    public static IEndpointRouteBuilder MapDownloadTicketAttachment(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/attachments/{id:guid}/download",
                async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var attachment = await sender.Send(
                        new DownloadTicketAttachmentQuery(id),
                        cancellationToken);

                    return attachment is null
                        ? Results.NotFound()
                        : Results.File(
                            attachment.Content,
                            attachment.ContentType,
                            attachment.FileName);
                })
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsView);

        return endpoints;
    }
}
