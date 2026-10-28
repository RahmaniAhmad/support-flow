using Shared.Authentication;
using Shared.Caching;
using Api.Features.TicketCategories;

namespace Api.Features.TicketCategories.Queries.GetTicketCategoryById;

public sealed class GetTicketCategoryByIdCacheKeyProvider
    : ICacheKeyProvider<GetTicketCategoryByIdQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetTicketCategoryByIdCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(GetTicketCategoryByIdQuery request)
    {
        return $"{GetGroup(request)}:category:{request.CategoryId}";
    }

    public string GetGroup(GetTicketCategoryByIdQuery request)
    {
        var companyId = _currentUser.CompanyId
            ?? throw TicketCategoryErrors.MissingCompanyContext();

        return $"ticket-categories:company:{companyId}";
    }
}
