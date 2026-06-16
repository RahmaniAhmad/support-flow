using MediatR;

namespace Api.Features.KnowledgeBase.GetArticles;

public sealed record GetArticlesQuery()
    : IRequest<List<GetArticlesResponse>>;