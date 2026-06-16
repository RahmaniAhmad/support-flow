using MediatR;

namespace Api.Features.Authentication.Login;

public sealed record LoginQuery(
    string Email,
    string Password) : IRequest<LoginResponse?>;
