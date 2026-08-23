using FluentValidation;

namespace Api.Features.KnowledgeBase.CreateArticle;

public sealed class CreateArticleCommandValidator
    : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(50_000);
    }
}