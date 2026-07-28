using MediatR;

namespace Api.Features.Users.ResetUserPassword;

public sealed record ResetUserPasswordCommand(
    Guid UserId,
    string Password
) : IRequest;