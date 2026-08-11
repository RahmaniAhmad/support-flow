using MediatR;

namespace Api.Features.AI.SemanticSearch;

public sealed record SemanticSearchQuery(
    string Query,
    int Limit = 5)
    : IRequest<SemanticSearchResponse>;