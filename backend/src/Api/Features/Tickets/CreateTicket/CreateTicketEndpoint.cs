using Infrastructure.Persistence;
using MediatR;
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
                    IMediator mediator,
                    CancellationToken cancellationToken) =>
                {

                    var command = new CreateTicketCommand(
                        currentUser.CompanyId,
                        currentUser.UserId,
                        request.Subject,
                        request.Description);

                    var ticketId = await mediator.Send(command, cancellationToken);

                    return Results.Created($"/tickets/{ticketId}", new { Id = ticketId });

                })
                .RequireAuthorization();

            return app;
        }
    }

}