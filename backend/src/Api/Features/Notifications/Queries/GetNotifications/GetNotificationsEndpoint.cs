using MediatR;

namespace Api.Features.Notifications.Queries.GetNotifications;

public static class GetNotificationsEndpoint
{
    public static void MapGetNotifications(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/notifications",
            GetNotificationsAsync)
            .RequireAuthorization();
    }

    private static async Task<IResult> GetNotificationsAsync(
      [AsParameters] GetNotificationsRequest request,
      ISender sender,
      CancellationToken cancellationToken)
    {
        var query = new GetNotificationsQuery(
            request.Page,
            request.PageSize);

        var notifications = await sender.Send(query, cancellationToken);

        return Results.Ok(notifications);
    }
}