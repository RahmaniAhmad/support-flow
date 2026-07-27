using MediatR;

namespace Api.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string? Phone) : IRequest;