using MediatR;

namespace Api.Features.Users.UpdateProfile;

public sealed record UpdateProfileCommand(
    string FirstName,
    string LastName,
    string? Phone)
    : IRequest;