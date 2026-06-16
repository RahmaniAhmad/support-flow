namespace Shared.Authentication;

public interface ICurrentUser
{
    Guid UserId { get; }

    Guid CompanyId { get; }

    string Email { get; }
    string Role { get; }
}
