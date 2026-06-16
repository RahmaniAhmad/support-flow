using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Authentication;

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

    public string Role =>
        _httpContextAccessor
            .HttpContext!
            .User
            .FindFirstValue(ClaimTypes.Role)!;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
}