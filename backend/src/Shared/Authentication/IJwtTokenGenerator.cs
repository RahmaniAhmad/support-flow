namespace Shared.Authentication;

public interface IJwtTokenGenerator
{
    string Generate(
        Guid userId,
        Guid companyId,
        string email,
        string role);
}
