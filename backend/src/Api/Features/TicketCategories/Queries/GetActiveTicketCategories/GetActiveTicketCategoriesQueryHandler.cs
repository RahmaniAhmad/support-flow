using Api.Errors;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategories;

public sealed class GetActiveTicketCategoriesQueryHandler
    : IRequestHandler<
        GetActiveTicketCategoriesQuery,
        IReadOnlyList<TicketCategoryResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetActiveTicketCategoriesQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<IReadOnlyList<TicketCategoryResponse>> Handle(
        GetActiveTicketCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
           ?? throw new ForbiddenException(
               CommonErrors.CompanyContextRequiredMessage,
               CommonErrors.CompanyContextRequiredCode);

        return await _db.TicketCategories
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId && x.IsActive)
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
