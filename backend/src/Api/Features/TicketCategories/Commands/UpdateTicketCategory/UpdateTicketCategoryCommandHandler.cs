using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.TicketCategories.Commands.UpdateTicketCategory;

public sealed class UpdateTicketCategoryCommandHandler
    : IRequestHandler<UpdateTicketCategoryCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public UpdateTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(
        UpdateTicketCategoryCommand request,
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

        var normalizedName = request.Name.Trim().ToLowerInvariant();
        var nameExists = await _db.TicketCategories.AnyAsync(
            x => x.CompanyId == companyId
                && x.Id != category.Id
                && EF.Property<string>(x, "NormalizedName") == normalizedName,
            cancellationToken);

        if (nameExists)
        {
            throw new ConflictException(
                TicketCategoryErrors.DuplicateMessage,
                TicketCategoryErrors.DuplicateCode);
        }

        category.UpdateDetails(request.Name, request.Description);
        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            when (TicketCategoryErrors.IsDuplicateNameViolation(exception))
        {
            throw new ConflictException(
                TicketCategoryErrors.DuplicateMessage,
                TicketCategoryErrors.DuplicateCode);
        }
    }
}
