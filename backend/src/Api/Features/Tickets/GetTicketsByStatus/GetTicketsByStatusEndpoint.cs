using Api.Features.Tickets.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Tickets.GetTicketsByStatus
{
    public static class GetTicketsByStatusEndpoint
    {
        public static IEndpointRouteBuilder MapGetTicketsByStatus(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/tickets/status/{status}",
                async (
                    TicketStatus status,
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var tickets = await db.Tickets
                        .Where(x =>
                            x.CompanyId == currentUser.CompanyId &&
                            x.Status == status)
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