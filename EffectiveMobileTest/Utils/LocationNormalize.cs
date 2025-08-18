using System.Text.RegularExpressions;

public static class LocationNormalizer
{
    public static (string Normalized, string[] Segments)? Normalize(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var s = input.Trim().Replace('\\', '/').ToLowerInvariant();
        if (!s.StartsWith('/')) s = "/" + s;
        if (s.Length > 1 && s.EndsWith('/')) s = s.TrimEnd('/');

        var parts = s.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var clean = new List<string>(parts.Length);
        var segmentRegex = new Regex("^[a-z0-9_-]+$", RegexOptions.Compiled);

        foreach (var part in parts)
        {
            if (string.IsNullOrEmpty(part)) continue;
            if (!segmentRegex.IsMatch(part)) return null;
            clean.Add(part);
        }

        var normalized = "/" + string.Join('/', clean);
        return (normalized, clean.ToArray());
    }
}