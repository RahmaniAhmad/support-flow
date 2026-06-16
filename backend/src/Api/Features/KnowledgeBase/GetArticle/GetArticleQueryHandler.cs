using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.GetArticle;

public sealed class GetArticleQueryHandler
    : IRequestHandler<GetArticleQuery, GetArticleResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetArticleQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<GetArticleResponse?> Handle(
        GetArticleQuery request,
        CancellationToken cancellationToken)
    {
        return await _db.KnowledgeArticles
            .Where(x =>
                x.Id == request.Id &&
                x.CompanyId == _currentUser.CompanyId)
            .Select(x => new GetArticleResponse(
                x.Id,
                x.Title,
                x.Content,
                x.CreatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);
    }
}