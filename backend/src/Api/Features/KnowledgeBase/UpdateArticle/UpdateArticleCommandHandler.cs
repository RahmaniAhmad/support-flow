using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
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

        article.Update(
                request.Title,
                request.Content);

        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}