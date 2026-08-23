using FluentValidation;

namespace Api.Features.KnowledgeBase.UpdateArticle;

public sealed class UpdateArticleCommandValidator
    : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(50_000);
    }
}