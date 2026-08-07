namespace Shared.Contracts.Sorting;

public static class SortDirectionParser
{
    public static SortDirection Parse(string? value)
    {
        return value?.ToLowerInvariant() switch
        {
            "asc" => SortDirection.Asc,
            "desc" => SortDirection.Desc,
            _ => SortDirection.Desc
        };
    }
}