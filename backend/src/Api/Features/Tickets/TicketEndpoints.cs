
using Api.Features.Tickets.Commands.AddComment;
using Api.Features.Tickets.Commands.AssignTicket;
using Api.Features.Tickets.Commands.AssignTicketCategory;
using Api.Features.Tickets.Commands.CloseTicket;
using Api.Features.Tickets.Commands.ChangeTicketPriority;
using Api.Features.Tickets.Commands.CreateTicket;
using Api.Features.Tickets.Commands.MoveTicketToPending;
using Api.Features.Tickets.Commands.RemoveTicketCategory;
using Api.Features.Tickets.Commands.ReopenTicket;
using Api.Features.Tickets.Commands.ResolveTicket;
using Api.Features.Tickets.Commands.StartProgress;
using Api.Features.Tickets.Attachments;
using Api.Features.Tickets.Queries.GetComments;
using Api.Features.Tickets.Queries.GetTicket;
using Api.Features.Tickets.Queries.GetTickets;
using Api.Features.Tickets.Queries.GetTicketsByStatus;
using Api.Features.Tickets.Queries.GetUnassignedTickets;

namespace Api.Features.Tickets;

public static class TicketEndpoints
{
    public static IEndpointRouteBuilder MapTicketEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetTickets();
        endpoints.MapGetTicket();
        endpoints.MapGetUnassignedTickets();
        endpoints.MapGetTicketsByStatus();
        endpoints.MapGetComments();

        endpoints.MapAddComment();
        endpoints.MapCreateTicket();
        endpoints.MapAssignTicket();
        endpoints.MapChangeTicketPriority();
        endpoints.MapAssignTicketCategory();
        endpoints.MapRemoveTicketCategory();
        endpoints.MapStartProgress();
        endpoints.MapMoveTicketToPending();
        endpoints.MapResolveTicket();
        endpoints.MapCloseTicket();
        endpoints.MapReopenTicket();
        endpoints.MapGroup("/api").MapTicketAttachmentEndpoints();

        return endpoints;
    }
}
