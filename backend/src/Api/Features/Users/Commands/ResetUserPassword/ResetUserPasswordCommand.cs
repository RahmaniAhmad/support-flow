using MediatR;

namespace Api.Features.Users.Commands.ResetUserPassword;

public sealed record ResetUserPasswordCommand(
    Guid UserId,
    string Password
) : IRequest;