using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.TicketCategories.Commands.DeactivateTicketCategory;

public sealed class DeactivateTicketCategoryCommandHandler
    : IRequestHandler<DeactivateTicketCategoryCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public DeactivateTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        DeactivateTicketCategoryCommand request,
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

        category.Deactivate();
        await _db.SaveChangesAsync(cancellationToken);
    }
}
