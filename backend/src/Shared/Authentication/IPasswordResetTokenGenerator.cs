namespace Shared.Authentication;

public interface IPasswordResetTokenGenerator
{
    string Generate();
    string Hash(string token);
}