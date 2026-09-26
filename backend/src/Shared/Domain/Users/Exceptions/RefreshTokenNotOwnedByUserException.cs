using Shared.Domain.Exceptions;

namespace Shared.Domain.Users.Exceptions;

public sealed class RefreshTokenNotOwnedByUserException()
    : DomainException(
        "The refresh token does not belong to this user.");