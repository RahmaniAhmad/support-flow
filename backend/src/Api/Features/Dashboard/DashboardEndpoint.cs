using Api.Features.Dashboard.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Dashboard
{
    public static class DashboardEndpoint
    {
        public static IEndpointRouteBuilder MapDashboard(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/dashboard",
                async (
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var companyId = currentUser.CompanyId;

                    var openTickets = await db.Tickets.CountAsync(
                        x =>
                            x.CompanyId == companyId &&
                            x.Status == TicketStatus.Open,
                        cancellationToken);

                    var inProgressTickets = await db.Tickets.CountAsync(
                        x =>
                            x.CompanyId == companyId &&
                            x.Status == TicketStatus.InProgress,
                        cancellationToken);

                    var resolvedTickets = await db.Tickets.CountAsync(
                        x =>
                            x.CompanyId == companyId &&
                            x.Status == TicketStatus.Resolved,
                        cancellationToken);

                    var unassignedTickets = await db.Tickets.CountAsync(
                        x =>
                            x.CompanyId == companyId &&
                            x.AssignedToUserId == null,
                        cancellationToken);

                    return Results.Ok(
                        new DashboardResponse(
                            openTickets,
                            inProgressTickets,
                            resolvedTickets,
                            unassignedTickets));
                })
                .RequireAuthorization();

            return app;
        }
    }
}