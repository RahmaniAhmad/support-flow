using Api.Features.Tickets.CreateTicket;
using Api.Features.Tickets.GetTickets;

namespace Api.Extensions
{

    public static class TicketEndpointExtensions
    {
        public static WebApplication MapTicketEndpoints(
            this WebApplication app)
        {
            app.MapCreateTicket();

            app.MapGetTickets();

            return app;
        }
    }
}