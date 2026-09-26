using Shared.Domain.Exceptions;

namespace Shared.Domain.Users.Exceptions;

public sealed class CompanyRequiredForUserRoleException(
    UserRole role)
    : DomainException(
        $"A company is required for user role '{role}'.")
{
    public UserRole Role { get; } = role;
}