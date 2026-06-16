using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.UpdateArticle;

public sealed class UpdateArticleCommandHandler
    : IRequestHandler<UpdateArticleCommand, bool>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public UpdateArticleCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(
        UpdateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = await _db.KnowledgeArticles
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    x.CompanyId == _currentUser.CompanyId,
                cancellationToken);

        if (article is null)
        {
            return false;
        }

        article.Title = request.Title;
        article.Content = request.Content;
        article.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}