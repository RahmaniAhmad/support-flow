using MediatR;

namespace Api.Features.KnowledgeBase.Queries.GetArticles;

public sealed record GetArticlesQuery()
    : IRequest<List<GetArticlesResponse>>;