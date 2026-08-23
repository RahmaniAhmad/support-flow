using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.KnowledgeBase.DeleteArticle;

public sealed class DeleteArticleCommandHandler
    : IRequestHandler<DeleteArticleCommand, bool>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public DeleteArticleCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(
        DeleteArticleCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
           ?? throw new ForbiddenException(
               CommonErrorMessages.CompanyContextRequired,
               CommonErrorCodes.CompanyContextRequired);

        var article = await _db.KnowledgeArticles
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.Id &&
                    x.CompanyId == companyId,
                cancellationToken);


        if (article is null)
        {
            throw new NotFoundException(
                KnowledgeBaseErrorMessages.ArticleNotFound,
                KnowledgeBaseErrorCodes.ArticleNotFound);
        }

        article.Delete();

        _db.KnowledgeArticles.Remove(article);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
