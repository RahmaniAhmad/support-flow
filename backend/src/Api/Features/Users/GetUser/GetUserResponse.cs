
namespace Api.Features.Users.GetUser;

public sealed record GetUserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string Role,
    bool IsActive,
    string? CompanyName,
    DateTime CreatedAtUtc);