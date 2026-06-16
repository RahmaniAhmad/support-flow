namespace Api.Features.KnowledgeBase.CreateArticle;

public sealed record CreateArticleRequest(
    string Title,
    string Content);
