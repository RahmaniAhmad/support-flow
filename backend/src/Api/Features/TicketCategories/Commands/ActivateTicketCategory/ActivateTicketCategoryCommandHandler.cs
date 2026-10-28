using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.TicketCategories.Commands.ActivateTicketCategory;

public sealed class ActivateTicketCategoryCommandHandler
    : IRequestHandler<ActivateTicketCategoryCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public ActivateTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        ActivateTicketCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
            ?? throw TicketCategoryErrors.MissingCompanyContext();
        var category = await _db.TicketCategories.FirstOrDefaultAsync(
            x => x.Id == request.CategoryId && x.CompanyId == companyId,
            cancellationToken)
            ?? throw new NotFoundException(
                TicketCategoryErrors.NotFoundMessage,
                TicketCategoryErrors.NotFoundCode);

        category.Activate();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
