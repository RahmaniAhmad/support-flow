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

    public Guid CompanyId =>
        Guid.Parse(
            _httpContextAccessor
                .HttpContext!
                .User
                .FindFirstValue("company_id")!);

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