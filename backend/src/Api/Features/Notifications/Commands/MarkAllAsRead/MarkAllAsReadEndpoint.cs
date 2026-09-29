using MediatR;

namespace Api.Features.Notifications.Commands.MarkAllAsRead;

public static class MarkAllAsReadEndpoint
{
    public static void MapMarkAllAsRead(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/notifications/read-all",
            MarkAllAsReadAsync)
            .RequireAuthorization();
    }


    private static async Task<IResult> MarkAllAsReadAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(
            new MarkAllAsReadCommand(),
            cancellationToken);


        return Results.NoContent();
    }
}