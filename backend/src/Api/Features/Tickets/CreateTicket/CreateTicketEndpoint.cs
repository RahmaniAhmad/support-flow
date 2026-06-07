using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain;
using Shared.Events;

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
                    IMediator mediator,
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

                    await mediator.Publish(
                        new TicketCreated(
                            ticket.Id,
                            ticket.CompanyId,
                            ticket.Subject),
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