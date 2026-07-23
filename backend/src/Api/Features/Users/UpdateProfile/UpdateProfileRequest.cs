namespace Api.Features.Users.UpdateProfile;

public sealed record UpdateProfileRequest(
    string FirstName,
    string LastName,
    string? Phone);