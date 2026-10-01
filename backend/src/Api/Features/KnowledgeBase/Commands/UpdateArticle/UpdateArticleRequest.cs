namespace Api.Features.KnowledgeBase.Commands.UpdateArticle;

public sealed record UpdateArticleRequest(
    string Title,
    string Content);
