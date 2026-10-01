namespace Api.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string? Phone);