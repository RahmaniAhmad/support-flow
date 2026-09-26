using Shared.Domain.Exceptions;

namespace Shared.Domain.Users.Exceptions;

public sealed class PasswordResetTokenAlreadyUsedException()
    : DomainException(
        "Password reset token has already been used.");