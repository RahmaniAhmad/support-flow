using MediatR;

namespace Api.Features.Users.ChangeUserStatus;

public record ChangeUserStatusCommand(
    Guid UserId,
    bool IsActive) : IRequest;