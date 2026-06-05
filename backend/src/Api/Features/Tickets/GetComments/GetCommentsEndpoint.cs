using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetComments
{
    public static class GetCommentsEndpoint
    {
        public static IEndpointRouteBuilder MapGetComments(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/tickets/{ticketId:guid}/comments",
                async (
                    Guid ticketId,
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var ticketExists = await db.Tickets
                        .AnyAsync(
                            x =>
                                x.Id == ticketId &&
                                x.CompanyId == currentUser.CompanyId,
                            cancellationToken);

                    if (!ticketExists)
                    {
                        return Results.NotFound();
                    }

                    var comments = await db.TicketComments
                        .Where(x => x.TicketId == ticketId)
                        .OrderBy(x => x.CreatedAtUtc)
                        .ToListAsync(cancellationToken);

                    return Results.Ok(comments);
                })
                .RequireAuthorization();

            return app;
        }
    }
}