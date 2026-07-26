namespace Api.Features.Users.GetAssignableUsers;

public sealed record GetAssignableUsersResponse(
    Guid Id,
    string FullName);