using FluentValidation;

namespace Api.Features.KnowledgeBase.Commands.DeleteArticle;

public sealed class DeleteArticleCommandValidator
    : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}