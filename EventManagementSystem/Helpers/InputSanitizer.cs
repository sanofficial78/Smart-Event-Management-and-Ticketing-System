namespace EventManagementSystem.Helpers;

/// <summary>
/// Sanitizes user input to prevent SQL injection. Use with parameterized queries (EF Core) for defense-in-depth.
/// Inputs containing SQL injection patterns are treated as empty - no DB damage.
/// </summary>
public static class InputSanitizer
{
    private static readonly string[] DangerousPatterns = { ";", "--", "/*", "*/", "xp_", "sp_", "drop ", "delete ", "insert ", "update ", "truncate ", "exec ", "execute " };

    /// <summary>
    /// Returns the input if safe, otherwise null. Use for search/filter string parameters.
    /// </summary>
    public static string? SanitizeSearchInput(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        var trimmed = input.Trim();
        if (trimmed.Length > 200) return null;
        var lower = trimmed.ToLowerInvariant();
        foreach (var p in DangerousPatterns)
        {
            if (lower.Contains(p)) return null;
        }
        return trimmed;
    }
}
