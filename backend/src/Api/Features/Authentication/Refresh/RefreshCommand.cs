using MediatR;

namespace Api.Features.Authentication.Refresh;

public sealed record RefreshCommand(
    string RefreshToken) : IRequest<RefreshResponse?>;