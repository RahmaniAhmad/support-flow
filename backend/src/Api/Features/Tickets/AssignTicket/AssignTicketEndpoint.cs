using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Tickets.AssignTicket
{
    public static class AssignTicketEndpoint
    {
        public static IEndpointRouteBuilder MapAssignTicket(
            this IEndpointRouteBuilder app)
        {
            app.MapPut(
                "/tickets/{ticketId:guid}/assign",
                async (
                    Guid ticketId,
                    AssignTicketRequest request,
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

                    ticket.AssignedToUserId = request.AssignedToUserId;
                    ticket.Status = TicketStatus.InProgress;
                    ticket.UpdatedAtUtc = DateTime.UtcNow;

                    await db.SaveChangesAsync(cancellationToken);

                    return Results.Ok(ticket);
                })
                .RequireAuthorization();

            return app;
        }
    }
}