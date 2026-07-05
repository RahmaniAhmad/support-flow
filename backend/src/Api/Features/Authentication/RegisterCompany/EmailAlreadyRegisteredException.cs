namespace Api.Features.Authentication.RegisterCompany;

public sealed class EmailAlreadyRegisteredException : Exception
{
    public EmailAlreadyRegisteredException()
        : base("Email is already registered.")
    {
    }
}