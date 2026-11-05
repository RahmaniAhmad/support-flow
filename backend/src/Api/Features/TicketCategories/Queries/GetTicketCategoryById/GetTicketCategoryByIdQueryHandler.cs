using Api.Errors;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.TicketCategories.Queries.GetTicketCategoryById;

public sealed class GetTicketCategoryByIdQueryHandler
    : IRequestHandler<GetTicketCategoryByIdQuery, TicketCategoryResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketCategoryByIdQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<TicketCategoryResponse> Handle(
        GetTicketCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
            ?? throw new ForbiddenException(
                CommonErrors.CompanyContextRequiredMessage,
                CommonErrors.CompanyContextRequiredCode);

        return await _db.TicketCategories
            .AsNoTracking()
            .Where(x =>
                x.Id == request.CategoryId
                && x.CompanyId == companyId)
            .Select(x => new TicketCategoryResponse(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedAtUtc,
                x.UpdatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                TicketCategoryErrors.NotFoundMessage,
                TicketCategoryErrors.NotFoundCode);
    }
}
