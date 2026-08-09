using MediatR;

namespace Api.Features.AI.SearchKnowledgeArticles;

public sealed record SearchKnowledgeArticlesQuery(
    string Query,
    int Limit = 5)
    : IRequest<SearchKnowledgeArticlesResponse>;