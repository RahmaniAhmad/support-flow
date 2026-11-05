using Shared.Authentication;
using Shared.Caching;
using Api.Exceptions;
using Api.Errors;

namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public sealed class GetTicketCategoriesCacheKeyProvider
    : ICacheKeyProvider<GetTicketCategoriesQuery>
{
    private readonly ICurrentUser _currentUser;

    public GetTicketCategoriesCacheKeyProvider(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public string GetKey(GetTicketCategoriesQuery request)
    {
        return $"{GetGroup(request)}:list:{request.Page}:{request.PageSize}:{request.Search}";
    }

    public string GetGroup(GetTicketCategoriesQuery request)
    {
        var companyId = _currentUser.CompanyId
        ?? throw new ForbiddenException(
            CommonErrors.CompanyContextRequiredMessage,
            CommonErrors.CompanyContextRequiredCode);

        return $"ticket-categories:company:{companyId}";
    }
}
