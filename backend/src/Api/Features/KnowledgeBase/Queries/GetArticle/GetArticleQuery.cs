using MediatR;

namespace Api.Features.KnowledgeBase.Queries.GetArticle;

public sealed record GetArticleQuery(Guid Id)
    : IRequest<GetArticleResponse?>;