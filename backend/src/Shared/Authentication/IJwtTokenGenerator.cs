using Shared.Domain.Users;

namespace Shared.Authentication;

public interface IJwtTokenGenerator
{
    string Generate(
        Guid userId,
        Guid companyId,
        string email,
        UserRole role);
}
