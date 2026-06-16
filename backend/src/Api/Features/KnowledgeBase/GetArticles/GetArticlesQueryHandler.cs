using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.GetArticles;

public sealed class GetArticlesQueryHandler
    : IRequestHandler<GetArticlesQuery, List<GetArticlesResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetArticlesQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetArticlesResponse>> Handle(
        GetArticlesQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.KnowledgeArticles
            .Where(x => x.CompanyId == _currentUser.CompanyId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new GetArticlesResponse(
                x.Id,
                x.Title,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}