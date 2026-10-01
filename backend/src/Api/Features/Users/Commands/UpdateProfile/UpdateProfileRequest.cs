namespace Api.Features.Users.Commands.UpdateProfile;

public sealed record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string? Phone);