using Api.Features.Tickets.Commands.AddComment;
using Api.Features.Tickets.Commands.AssignTicket;
using Api.Features.Tickets.Commands.CloseTicket;
using Api.Features.Tickets.Commands.CreateTicket;
using Api.Features.Tickets.Queries.GetComments;
using Api.Features.Tickets.Queries.GetTicket;
using Api.Features.Tickets.Queries.GetTickets;
using Api.Features.Tickets.Queries.GetTicketsByStatus;
using Api.Features.Tickets.Queries.GetUnassignedTickets;
using Api.Features.Tickets.Commands.MoveTicketToPending;
using Api.Features.Tickets.Commands.ReopenTicket;
using Api.Features.Tickets.Commands.ResolveTicket;
using Api.Features.Tickets.Commands.StartProgress;

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
