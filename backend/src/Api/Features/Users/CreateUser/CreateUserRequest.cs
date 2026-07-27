using Shared.Domain.Users;

namespace Api.Features.Users.CreateUser;

public sealed record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Phone,
    UserRole Role);