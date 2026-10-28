using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.Companies;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler
    : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public CreateTicketCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task<Guid> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
                   ?? throw new BadRequestException(
                       CommonErrorMessages.CompanyContextRequired,
                       CommonErrorCodes.CompanyContextRequired);

        if (request.CategoryId is Guid categoryId)
        {
            var categoryIsAvailable = await _db.TicketCategories
                .AnyAsync(
                    x => x.Id == categoryId
                        && x.CompanyId == companyId
                        && x.IsActive,
                    cancellationToken);

            if (!categoryIsAvailable)
            {
                throw new BadRequestException(
                    "The selected ticket category is unavailable.",
                    "TICKET_CATEGORY_UNAVAILABLE");
            }
        }

        var counter = await _db.CompanyTicketCounters
          .FirstOrDefaultAsync(
              x => x.CompanyId == companyId,
              cancellationToken);

        if (counter is null)
        {
            counter = CompanyTicketCounter.Create(companyId);

            _db.CompanyTicketCounters.Add(counter);
        }

        var ticketNumber = counter.GetNextNumber();


        var ticket = Ticket.Create(
            companyId,
            _currentUser.UserId,
            request.Subject,
            request.Description,
            request.CategoryId);


        ticket.AssignTicketNumber(ticketNumber);


        _db.Tickets.Add(ticket);

        await _db.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}