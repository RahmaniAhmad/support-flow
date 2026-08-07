using Api.Features.Tickets.AddComment;
using Api.Features.Tickets.AssignTicket;
using Api.Features.Tickets.CloseTicket;
using Api.Features.Tickets.CreateTicket;
using Api.Features.Tickets.GetComments;
using Api.Features.Tickets.GetTicket;
using Api.Features.Tickets.GetTickets;
using Api.Features.Tickets.GetTicketsByStatus;
using Api.Features.Tickets.GetUnassignedTickets;
using Api.Features.Tickets.MoveTicketToPending;
using Api.Features.Tickets.ReopenTicket;
using Api.Features.Tickets.ResolveTicket;
using Api.Features.Tickets.StartProgress;

namespace Api.Extensions;


public static class TicketEndpointExtensions
{
    public static WebApplication MapTicketEndpoints(
        this WebApplication app)
    {
        // Queries
        app.MapGetTickets();
        app.MapGetTicket();
        app.MapGetUnassignedTickets();
        app.MapGetTicketsByStatus();
        app.MapGetComments();

        // Ticket
        app.MapCreateTicket();
        app.MapAssignTicket();
        app.MapStartProgress();
        app.MapMoveTicketToPending();
        app.MapResolveTicket();
        app.MapCloseTicket();
        app.MapReopenTicket();

        // Comment
        app.MapAddComment();

        return app;
    }
}
