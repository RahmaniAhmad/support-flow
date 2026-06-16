using MediatR;

namespace Api.Features.Tickets.GetComments;

public static class GetCommentsEndpoint
{
    public static IEndpointRouteBuilder MapGetComments(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/{ticketId:guid}/comments",
            async (
                Guid ticketId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetTicketCommentsQuery(ticketId),
                    cancellationToken);

                return result is null
                    ? Results.NotFound()
                    : Results.Ok(result);
            })
            .RequireAuthorization();

        return app;
    }
}
