namespace Infrastructure.Authentication;

public sealed class SuperAdminOptions
{
    public const string SectionName = "SuperAdmin";

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}