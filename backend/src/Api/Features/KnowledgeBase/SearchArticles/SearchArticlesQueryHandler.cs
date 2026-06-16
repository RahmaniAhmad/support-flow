using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.SearchArticles;

public sealed class SearchArticlesQueryHandler
    : IRequestHandler<SearchArticlesQuery, List<SearchArticlesResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public SearchArticlesQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<SearchArticlesResponse>> Handle(
        SearchArticlesQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return new List<SearchArticlesResponse>();

        var q = request.Query.Trim();

        return await _db.KnowledgeArticles
            .Where(x =>
                x.CompanyId == _currentUser.CompanyId &&
                (
                    EF.Functions.ILike(x.Title, $"%{q}%") ||
                    EF.Functions.ILike(x.Content, $"%{q}%")
                ))
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new SearchArticlesResponse(
                x.Id,
                x.Title,
                x.Content,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}