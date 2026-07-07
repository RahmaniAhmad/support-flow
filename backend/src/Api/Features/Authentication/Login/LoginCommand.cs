using MediatR;

namespace Api.Features.Authentication.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<LoginResponse?>;
