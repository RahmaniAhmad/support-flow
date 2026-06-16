using MediatR;

namespace Api.Features.KnowledgeBase.CreateArticle;

public sealed record CreateArticleCommand(
    string Title,
    string Content)
    : IRequest<CreateArticleResponse>;
