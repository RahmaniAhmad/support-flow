using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.AddComment;

public static class AddCommentEndpoint
{
    public static IEndpointRouteBuilder MapAddComment(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/tickets/{ticketId:guid}/comments",
            async (
                Guid ticketId,
                AddCommentRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new AddCommentCommand(
                    ticketId,
                    request.Content);

                var commentId = await sender.Send(command, cancellationToken);

                return Results.Ok(new { Id = commentId });
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsComment);

        return app;
    }
}
