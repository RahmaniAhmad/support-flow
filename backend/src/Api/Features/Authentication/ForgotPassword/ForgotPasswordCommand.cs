using MediatR;

namespace Api.Features.Authentication.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Email) : IRequest;