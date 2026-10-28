using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.Tickets;

namespace Api.Features.TicketCategories.Commands.CreateTicketCategory;

public sealed class CreateTicketCategoryCommandHandler
    : IRequestHandler<CreateTicketCategoryCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CreateTicketCategoryCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreateTicketCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
            ?? throw TicketCategoryErrors.MissingCompanyContext();
        var normalizedName = request.Name.Trim().ToLowerInvariant();

        var nameExists = await _db.TicketCategories.AnyAsync(
            x => x.CompanyId == companyId
                && EF.Property<string>(x, "NormalizedName") == normalizedName,
            cancellationToken);

        if (nameExists)
        {
            throw new ConflictException(
                TicketCategoryErrors.DuplicateMessage,
                TicketCategoryErrors.DuplicateCode);
        }

        var category = TicketCategory.Create(
            companyId,
            request.Name,
            request.Description);

        _db.TicketCategories.Add(category);
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

        return category.Id;
    }
}
