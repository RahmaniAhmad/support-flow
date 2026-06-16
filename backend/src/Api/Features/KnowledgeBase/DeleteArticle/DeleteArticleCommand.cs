using MediatR;

namespace Api.Features.KnowledgeBase.DeleteArticle;

public sealed record DeleteArticleCommand(Guid Id)
    : IRequest<bool>;
