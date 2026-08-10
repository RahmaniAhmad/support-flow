namespace Api.Features.AI.SemanticSearch;

public sealed record SemanticSearchRequest(
    string Query,
    int Limit = 5);