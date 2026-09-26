using Shared.Domain.Exceptions;

namespace Shared.Domain.Users.Exceptions;

public sealed class RefreshTokenAlreadyRevokedException()
    : DomainException(
        "Refresh token has already been revoked.");