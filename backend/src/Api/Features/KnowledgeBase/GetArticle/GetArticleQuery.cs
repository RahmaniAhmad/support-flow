using MediatR;

namespace Api.Features.KnowledgeBase.GetArticle;

public sealed record GetArticleQuery(Guid Id)
    : IRequest<GetArticleResponse?>;