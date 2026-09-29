using MediatR;

namespace Api.Features.Notifications.Queries.GetUnreadCount;

public static class GetUnreadCountEndpoint
{
    public static void MapGetUnreadCount(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/notifications/unread-count",
            GetUnreadCountAsync)
            .RequireAuthorization();
    }


    private static async Task<IResult> GetUnreadCountAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result =
            await sender.Send(
                new GetUnreadCountQuery(),
                cancellationToken);


        return Results.Ok(result);
    }
}