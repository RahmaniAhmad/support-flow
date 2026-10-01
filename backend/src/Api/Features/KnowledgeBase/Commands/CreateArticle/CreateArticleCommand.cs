using MediatR;

namespace Api.Features.KnowledgeBase.Commands.CreateArticle;

public sealed record CreateArticleCommand(
    string Title,
    string Content)
    : IRequest<CreateArticleResponse>;
