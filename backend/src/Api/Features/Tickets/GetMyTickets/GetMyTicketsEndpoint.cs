using Api.Features.Tickets.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetMyTickets
{
    public static class GetMyTicketsEndpoint
    {
        public static IEndpointRouteBuilder MapGetMyTickets(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/tickets/my",
                async (
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var tickets = await db.Tickets
                        .Where(x =>
                            x.CompanyId == currentUser.CompanyId &&
                            x.AssignedToUserId == currentUser.UserId)
                        .OrderByDescending(x => x.CreatedAtUtc)
                         .Select(x =>
                         new TicketResponse(
                            x.Id,
                            x.Subject,
                            x.Description,
                            x.Status.ToString(),
                            x.AssignedToUserId,
                            x.CreatedAtUtc))
                        .ToListAsync(cancellationToken);

                    return Results.Ok(tickets);
                })
                .RequireAuthorization();

            return app;
        }
    }
}