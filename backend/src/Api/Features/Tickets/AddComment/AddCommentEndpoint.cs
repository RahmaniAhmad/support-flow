using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Tickets.AddComment
{
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
                    CancellationToken cancellationToken) =>
                {
                    var ticket = await db.Tickets
                        .FirstOrDefaultAsync(
                            x =>
                                x.Id == ticketId &&
                                x.CompanyId == currentUser.CompanyId,
                            cancellationToken);

                    if (ticket is null)
                    {
                        return Results.NotFound();
                    }

                    var comment = new TicketComment
                    {
                        Id = Guid.NewGuid(),
                        TicketId = ticket.Id,
                        AuthorUserId = currentUser.UserId,
                        Content = request.Content,
                        CreatedAtUtc = DateTime.UtcNow
                    };

                    db.TicketComments.Add(comment);

                    await db.SaveChangesAsync(
                        cancellationToken);

                    return Results.Ok(comment);
                })
                .RequireAuthorization();

            return app;
        }
    }
}