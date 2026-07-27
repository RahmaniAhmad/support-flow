namespace Api.Features.Users.GetUsers;

public sealed record GetUsersResponse(
    Guid Id,
    string? CompanyName,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    string Role,
    bool IsActive,
    DateTime CreatedAtUtc);