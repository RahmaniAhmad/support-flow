using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Phone,
    UserRole Role) : IRequest<CreateUserResponse>;