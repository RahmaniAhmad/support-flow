using Shared.Authentication;
using Shared.Caching;
using Api.Features.TicketCategories;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategories;

public sealed class GetActiveTicketCategoriesCacheKeyProvider
    : ICacheKeyProvider<GetActiveTicketCategoriesQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetActiveTicketCategoriesCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(GetActiveTicketCategoriesQuery request)
    {
        return $"{GetGroup(request)}:active";
    }

    public string GetGroup(GetActiveTicketCategoriesQuery request)
    {
        var companyId = _currentUser.CompanyId
            ?? throw TicketCategoryErrors.MissingCompanyContext();

        return $"ticket-categories:company:{companyId}";
    }
}
