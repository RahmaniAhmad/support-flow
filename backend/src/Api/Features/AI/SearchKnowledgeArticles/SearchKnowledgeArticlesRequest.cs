namespace Api.Features.AI.SearchKnowledgeArticles;

public sealed record SearchKnowledgeArticlesRequest(
    string Query,
    int Limit = 5);