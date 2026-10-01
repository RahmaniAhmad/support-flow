namespace Api.Features.Users.Queries.GetAssignableUsers;

public sealed record GetAssignableUsersResponse(
    Guid Id,
    string FullName);