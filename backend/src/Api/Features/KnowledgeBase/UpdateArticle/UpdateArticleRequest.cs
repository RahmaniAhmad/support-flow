namespace Api.Features.KnowledgeBase.UpdateArticle
{
    public sealed record UpdateArticleRequest(
        string Title,
        string Content);
}