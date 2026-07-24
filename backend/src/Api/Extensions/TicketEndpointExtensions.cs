using Api.Features.Tickets.AddComment;
using Api.Features.Tickets.AssignTicket;
using Api.Features.Tickets.CloseTicket;
using Api.Features.Tickets.CreateTicket;
using Api.Features.Tickets.GetComments;
using Api.Features.Tickets.GetMyTickets;
using Api.Features.Tickets.GetTicket;
using Api.Features.Tickets.GetTickets;
using Api.Features.Tickets.GetTicketsByStatus;
using Api.Features.Tickets.GetUnassignedTickets;
using Api.Features.Tickets.ReopenTicket;

namespace Api.Extensions;


public static class TicketEndpointExtensions
{
    public static WebApplication MapTicketEndpoints(
        this WebApplication app)
    {
        app.MapGetTickets();
        app.MapGetTicket();
        app.MapGetMyTickets();
        app.MapGetUnassignedTickets();
        app.MapGetTicketsByStatus();

        app.MapAssignTicket();
        app.MapCreateTicket();
        app.MapCloseTicket();
        app.MapReopenTicket();

        app.MapAddComment();
        app.MapGetComments();

        return app;
    }
}
