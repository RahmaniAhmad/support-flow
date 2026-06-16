using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.KnowledgeBase.CreateArticle;

public sealed class CreateArticleCommandHandler
    : IRequestHandler<CreateArticleCommand, CreateArticleResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public CreateArticleCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<CreateArticleResponse> Handle(
        CreateArticleCommand request,
        CancellationToken cancellationToken)
    {
        var article = new KnowledgeArticle
        {
            CompanyId = _currentUser.CompanyId,
            Title = request.Title,
            Content = request.Content,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.KnowledgeArticles.Add(article);

        await _db.SaveChangesAsync(cancellationToken);

        return new CreateArticleResponse(article.Id, article.Title);
    }
}
