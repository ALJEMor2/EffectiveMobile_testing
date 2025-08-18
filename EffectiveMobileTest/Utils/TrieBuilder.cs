public static class TrieBuilder
{
    public static (TrieNode Root, ImportReport Report) BuildTrieFromText(string text)
    {
        var report = new ImportReport();
        var map = new Dictionary<string[], HashSet<string>>(new SegmentsComparer());

        using var reader = new StringReader(text ?? string.Empty);
        string? line;
        int lineNumber = 0;

        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            var trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed)) { report.LinesIgnored++; continue; }

            int colonIndex = trimmed.IndexOf(':');
            if (colonIndex <= 0 || colonIndex == trimmed.Length - 1) { report.LinesIgnored++; continue; }

            string name = trimmed[..colonIndex].Trim();
            string locations = trimmed[(colonIndex + 1)..].Trim();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(locations)) { report.LinesIgnored++; continue; }

            var locationList = locations.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            int validLocations = 0;
            foreach (var raw in locationList)
            {
                var normalized = LocationNormalizer.Normalize(raw);
                if (normalized is null) continue;
                validLocations++;

                var key = normalized.Value.Segments;
                if (!map.TryGetValue(key, out var set))
                {
                    set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    map[key] = set;
                }
                set.Add(name);
            }

            if (validLocations > 0)
            {
                report.LinesProcessed++;
                report.TotalPlatforms++;
            }
            else
            {
                report.LinesIgnored++;
            }
        }

        var root = TrieNode.Build(map);
        return (root, report);
    }
}