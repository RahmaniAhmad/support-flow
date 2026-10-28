using Api.Features.TicketCategories;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts;

namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public sealed class GetTicketCategoriesQueryHandler
    : IRequestHandler<GetTicketCategoriesQuery, PagedResult<TicketCategoryResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketCategoriesQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<TicketCategoryResponse>> Handle(
        GetTicketCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
            ?? throw TicketCategoryErrors.MissingCompanyContext();

        var query = _db.TicketCategories
            .AsNoTracking()
            .Where(x => x.CompanyId == companyId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(
                    x.Name,
                    $"%{request.Search.Trim()}%"));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderBy(x => EF.Property<string>(x, "NormalizedName"))
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new TicketCategoryResponse(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedAtUtc,
                x.UpdatedAtUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<TicketCategoryResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize);
    }
}
