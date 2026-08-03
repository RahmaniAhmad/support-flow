namespace Shared.Caching;

public static class CacheExpiration
{
    public static readonly TimeSpan Default =
         TimeSpan.FromMinutes(5);

    public static readonly TimeSpan Dashboard =
        TimeSpan.FromMinutes(2);

    public static readonly TimeSpan Ticket =
        TimeSpan.FromMinutes(10);

    public static readonly TimeSpan Comments =
        TimeSpan.FromMinutes(5);

    public static readonly TimeSpan KnowledgeArticle =
   TimeSpan.FromMinutes(30);
}