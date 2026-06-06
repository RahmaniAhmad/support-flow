using Api.Features.Tickets.AddComment;
using Api.Features.Tickets.AssignTicket;
using Api.Features.Tickets.CreateTicket;
using Api.Features.Tickets.GetComments;
using Api.Features.Tickets.GetTickets;
using Api.Features.Tickets.UpdateStatus;

namespace Api.Extensions
{

    public static class TicketEndpointExtensions
    {
        public static WebApplication MapTicketEndpoints(
            this WebApplication app)
        {
            app.MapCreateTicket();
            app.MapGetTickets();
            app.MapAddComment();
            app.MapGetComments();
            app.MapAssignTicket();
            app.MapUpdateTicketStatus();

            return app;
        }
    }
}