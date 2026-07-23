using Shared.Domain.Users;

namespace Api.Features.Users.GetProfile;

public sealed record GetProfileResponse(
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    UserRole Role,
    string? CompanyName);