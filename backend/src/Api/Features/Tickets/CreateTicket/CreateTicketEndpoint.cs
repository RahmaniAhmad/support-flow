using Infrastructure.Persistence;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Tickets.CreateTicket
{

    public static class CreateTicketEndpoint
    {
        public static IEndpointRouteBuilder MapCreateTicket(
            this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/tickets",
                async (
                    CreateTicketRequest request,
                    SupportFlowDbContext db,
                    ICurrentUser currentUser,
                    CancellationToken cancellationToken) =>
                {
                    var ticket = new Ticket
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = currentUser.CompanyId,
                        CreatedByUserId = currentUser.UserId,
                        Subject = request.Subject,
                        Description = request.Description,
                        Status = TicketStatus.Open,
                        CreatedAtUtc = DateTime.UtcNow
                    };

                    db.Tickets.Add(ticket);

                    await db.SaveChangesAsync(
                        cancellationToken);

                    return Results.Created(
                        $"/tickets/{ticket.Id}",
                        ticket);
                })
                .RequireAuthorization();

            return app;
        }
    }
}