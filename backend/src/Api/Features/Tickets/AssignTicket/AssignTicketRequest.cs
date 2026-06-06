namespace Api.Features.Tickets.AssignTicket
{
    public sealed record AssignTicketRequest(
        Guid AssignedToUserId);
}