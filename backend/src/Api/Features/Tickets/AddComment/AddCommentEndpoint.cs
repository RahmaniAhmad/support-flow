using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;

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
                SupportFlowDbContext db,
                ICurrentUser currentUser,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new AddCommentCommand(
                    ticketId,
                    currentUser.UserId,
                    currentUser.CompanyId,
                    request.Content);

                var commentId = await sender.Send(command, cancellationToken);

                return Results.Ok(new { Id = commentId });
            })
            .RequireAuthorization();

        return app;
    }
}
