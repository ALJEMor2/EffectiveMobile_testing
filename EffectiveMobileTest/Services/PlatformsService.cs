using System.Collections.Immutable;


public class PlatformsService(ILogger<PlatformsService> logger) : IPlatformsService
{
    private volatile TrieNode _currentRoot = TrieNode.Empty;

    public ImportReport LoadFromText(string text)
    {
        logger.LogInformation("Starting to load platforms from text.");

        var (root, report) = TrieBuilder.BuildTrieFromText(text);
        _currentRoot = root;

        logger.LogInformation("Finished loading platforms.");

        return report;
    }

    public IEnumerable<string> Search(string location)
    {
        var normalizedLocation = LocationNormalizer.Normalize(location);

        if (normalizedLocation is null)
        {
            logger.LogWarning("Invalid location format: {Location}", location);
            return Enumerable.Empty<string>();
        }

        var node = _currentRoot.FindDeepest(normalizedLocation.Value.Segments);
        var platforms = node?.CumulativePlatforms ?? ImmutableSortedSet<string>.Empty;

        logger.LogInformation("Platforms found for location {Location}: {Platforms}", location, string.Join(", ", platforms));

        return platforms;
    }
}