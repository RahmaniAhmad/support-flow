using MediatR;

namespace Api.Features.KnowledgeBase.SearchArticles;

public sealed record SearchArticlesQuery(string Query)
    : IRequest<List<SearchArticlesResponse>>;