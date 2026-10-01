using MediatR;

namespace Api.Features.Users.Commands.ChangeUserStatus;

public record ChangeUserStatusCommand(
    Guid UserId,
    bool IsActive) : IRequest;