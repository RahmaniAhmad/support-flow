using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.TicketCategories.Commands.AssignTicketCategory;

public sealed class AssignTicketCategoryCommandHandler
    : IRequestHandler<AssignTicketCategoryCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccessService;

    public AssignTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccessService)
    {
        _db = db;
        _ticketAccessService = ticketAccessService;
    }

    public async Task Handle(
        AssignTicketCategoryCommand request,
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

        var category = await _db.TicketCategories
            .FirstOrDefaultAsync(
                x => x.Id == request.CategoryId
                    && x.CompanyId == ticket.CompanyId
                    && x.IsActive,
                cancellationToken)
            ?? throw new NotFoundException(
                TicketCategoryErrors.NotFoundMessage,
                TicketCategoryErrors.NotFoundCode);

        ticket.AssignCategory(category.Id);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
