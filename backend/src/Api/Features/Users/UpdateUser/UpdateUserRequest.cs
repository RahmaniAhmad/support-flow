namespace Api.Features.Users.UpdateUser;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string? Phone);