using MediatR;

namespace Api.Features.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(
    string Token,
    string Password) : IRequest;