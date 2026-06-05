using Microsoft.EntityFrameworkCore;

using Infrastructure.Persistence;

using Shared.Authentication;

namespace Api.Features.Tickets.GetTickets
{

    public static class GetTicketsEndpoint
    {
        public static IEndpointRouteBuilder MapGetTickets(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/tickets",
                async (
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var tickets = await db.Tickets
                        .Where(x =>
                            x.CompanyId ==
                            currentUser.CompanyId)
                        .OrderByDescending(x =>
                            x.CreatedAtUtc)
                        .ToListAsync(
                            cancellationToken);

                    return Results.Ok(tickets);
                })
                .RequireAuthorization();

            return app;
        }
    }
}