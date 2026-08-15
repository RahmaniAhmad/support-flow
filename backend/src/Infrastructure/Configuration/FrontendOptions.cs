namespace Infrastructure.Configuration;

public sealed class FrontendOptions
{
    public const string SectionName = "Frontend";

    public string Url { get; set; } = string.Empty;
}