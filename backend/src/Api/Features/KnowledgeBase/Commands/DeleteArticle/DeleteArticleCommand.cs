using MediatR;

namespace Api.Features.KnowledgeBase.Commands.DeleteArticle;

public sealed record DeleteArticleCommand(Guid Id)
    : IRequest<bool>;
