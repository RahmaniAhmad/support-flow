using MediatR;

namespace Api.Features.KnowledgeBase.Queries.SearchArticles;

public sealed record SearchArticlesQuery(string Query)
    : IRequest<List<SearchArticlesResponse>>;