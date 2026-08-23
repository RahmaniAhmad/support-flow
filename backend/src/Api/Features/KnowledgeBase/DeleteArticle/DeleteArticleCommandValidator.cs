using FluentValidation;

namespace Api.Features.KnowledgeBase.DeleteArticle;

public sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}