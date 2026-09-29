using MediatR;

namespace Api.Features.Notifications.Commands.MarkAsRead;

public static class MarkAsReadEndpoint
{
    public static void MapMarkAsRead(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/notifications/{id:guid}/read",
            MarkAsReadAsync)
            .RequireAuthorization();
    }


    private static async Task<IResult> MarkAsReadAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new MarkAsReadCommand(id),
            cancellationToken);


        return Results.NoContent();
    }
}