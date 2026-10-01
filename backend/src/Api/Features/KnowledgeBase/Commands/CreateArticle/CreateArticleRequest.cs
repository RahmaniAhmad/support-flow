namespace Api.Features.KnowledgeBase.Commands.CreateArticle;

public sealed record CreateArticleRequest(
    string Title,
    string Content);
