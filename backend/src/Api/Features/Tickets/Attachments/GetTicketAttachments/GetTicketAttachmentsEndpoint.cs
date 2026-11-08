using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Attachments.GetTicketAttachments;

public static class GetTicketAttachmentsEndpoint
{
    public static IEndpointRouteBuilder MapGetTicketAttachments(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/tickets/{ticketId:guid}/attachments",
                async (
                    Guid ticketId,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    var attachments = await sender.Send(
                        new GetTicketAttachmentsQuery(ticketId),
                        cancellationToken);

                    return attachments is null
                        ? Results.NotFound()
                        : Results.Ok(attachments);
                })
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsView);

        return endpoints;
    }
}
