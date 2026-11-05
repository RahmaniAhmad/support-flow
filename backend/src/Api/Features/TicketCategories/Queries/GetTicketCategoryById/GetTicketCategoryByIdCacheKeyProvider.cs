using Shared.Authentication;
using Shared.Caching;
using Api.Exceptions;
using Api.Errors;

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
           ?? throw new ForbiddenException(
               CommonErrors.CompanyContextRequiredMessage,
               CommonErrors.CompanyContextRequiredCode);

        return $"ticket-categories:company:{companyId}";
    }
}
