using Shared.Authentication;
using Shared.Caching;

namespace Api.Features.KnowledgeBase.GetArticle;

public sealed class GetArticleCacheKeyProvider
    : ICacheKeyProvider<GetArticleQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetArticleCacheKeyProvider(
        ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }


    public string GetKey(
        GetArticleQuery request)
    {
        var companyId = _currentUser.CompanyId
            ?? throw new InvalidOperationException(
                "Company id is required.");

        return $"knowledge-articles:company:{companyId}:article:{request.Id}";
    }


    public TimeSpan? Expiration => CacheExpiration.KnowledgeArticle;
}