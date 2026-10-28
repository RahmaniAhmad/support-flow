using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.TicketCategories.Commands.RemoveTicketCategory;

public sealed class RemoveTicketCategoryCommandHandler
    : IRequestHandler<RemoveTicketCategoryCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccessService;

    public RemoveTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccessService)
    {
        _db = db;
        _ticketAccessService = ticketAccessService;
    }

    public async Task Handle(
        RemoveTicketCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccessService
            .ApplyTicketAccessFilter(_db.Tickets)
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Ticket was not found.",
                "TICKET_NOT_FOUND");

        ticket.RemoveCategory();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
