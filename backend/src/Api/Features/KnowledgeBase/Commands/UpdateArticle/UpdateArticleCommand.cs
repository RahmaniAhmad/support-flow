using MediatR;

namespace Api.Features.KnowledgeBase.Commands.UpdateArticle;

public sealed record UpdateArticleCommand(
    Guid Id,
    string Title,
    string Content)
    : IRequest<bool>;