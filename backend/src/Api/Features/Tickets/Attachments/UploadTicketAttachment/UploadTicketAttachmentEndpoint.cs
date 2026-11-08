using Api.Authorization;
using Api.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Attachments.UploadTicketAttachment;

public static class UploadTicketAttachmentEndpoint
{
    private const long MaximumRequestSize =
        TicketAttachmentValidator.MaximumFileSize + 1024 * 1024;

    public static IEndpointRouteBuilder MapUploadTicketAttachment(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/tickets/{ticketId:guid}/attachments",
                async (
                    Guid ticketId,
                    IFormFile file,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var attachment = await sender.Send(
                        new UploadTicketAttachmentCommand(ticketId, file),
                        cancellationToken);

                    return Results.Created(
                        $"/api/attachments/{attachment.Id}/download",
                        attachment);
                })
            .DisableAntiforgery()
            .WithMetadata(new RequestSizeLimitAttribute(MaximumRequestSize))
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsCreate);

        return endpoints;
    }
}
