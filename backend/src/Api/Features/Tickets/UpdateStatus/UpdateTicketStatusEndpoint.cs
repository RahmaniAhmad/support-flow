using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.UpdateStatus
{
    public static class UpdateTicketStatusEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateTicketStatus(
            this IEndpointRouteBuilder app)
        {
            app.MapPut(
                "/tickets/{ticketId:guid}/status",
                async (
                    Guid ticketId,
                    UpdateTicketStatusRequest request,
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

                    ticket.Status = request.Status;
                    ticket.UpdatedAtUtc = DateTime.UtcNow;

                    await db.SaveChangesAsync(cancellationToken);

                    return Results.Ok(ticket);
                })
                .RequireAuthorization();

            return app;
        }
    }
}