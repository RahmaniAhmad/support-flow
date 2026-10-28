using Api.Authorization;
using Api.Exceptions;
using Api.Features.TicketCategories.Queries;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;

public sealed class GetActiveTicketCategoriesForTicketQueryHandler
    : IRequestHandler<
        GetActiveTicketCategoriesForTicketQuery,
        IReadOnlyList<TicketCategoryResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccessService;

    public GetActiveTicketCategoriesForTicketQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccessService)
    {
        _db = db;
        _ticketAccessService = ticketAccessService;
    }

    public async Task<IReadOnlyList<TicketCategoryResponse>> Handle(
        GetActiveTicketCategoriesForTicketQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccessService
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Ticket was not found.",
                "TICKET_NOT_FOUND");

        return await _db.TicketCategories
            .AsNoTracking()
            .Where(x => x.CompanyId == ticket.CompanyId && x.IsActive)
            .OrderBy(x => EF.Property<string>(x, "NormalizedName"))
            .Select(x => new TicketCategoryResponse(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedAtUtc,
                x.UpdatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}
