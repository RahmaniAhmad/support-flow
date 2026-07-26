using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Infrastructure.Authentication;

public sealed class CurrentUser : ICurrentUser
{
    public Guid UserId =>
        Guid.Parse(
            _httpContextAccessor
                .HttpContext!
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier)!);

    public Guid? CompanyId
    {
        get
        {
            var companyId =
                _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue("company_id");

            if (string.IsNullOrWhiteSpace(companyId))
                return null;

            return Guid.Parse(companyId);
        }

    }

    public string Email =>
        _httpContextAccessor
            .HttpContext!
            .User
            .FindFirstValue(ClaimTypes.Email)!;

    public UserRole Role
    {
        get
        {
            var roleValue = _httpContextAccessor.HttpContext!
                .User.FindFirstValue(ClaimTypes.Role);

            if (!Enum.TryParse<UserRole>(roleValue, out var role))
            {
                throw new InvalidOperationException(
                    $"Invalid role claim: {roleValue}");
            }

            return role;
        }
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}