using Api.Filters;
using MediatR;

namespace Api.Features.Tickets.Attachments.DeleteTicketAttachment;

public static class DeleteTicketAttachmentEndpoint
{
    public static IEndpointRouteBuilder MapDeleteTicketAttachment(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/attachments/{id:guid}",
                async (
                    Guid id,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new DeleteTicketAttachmentCommand(id),
                        cancellationToken);

                    return Results.NoContent();
                })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization();

        return endpoints;
    }
}
