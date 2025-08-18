using System.Collections.Immutable;
using Microsoft.Extensions.Logging;

public class PlatformsService : IPlatformsService
{
    private readonly ILogger<PlatformsService> _logger;
    private volatile TrieNode _currentRoot = TrieNode.Empty;

    public PlatformsService(ILogger<PlatformsService> logger)
    {
        _logger = logger;
    }

    public ImportReport LoadFromText(string text)
    {
        _logger.LogInformation("Starting to load platforms from text.");

        var (root, report) = TrieBuilder.BuildTrieFromText(text);
        _currentRoot = root;

        _logger.LogInformation("Finished loading platforms.");

        return report;
    }

    public IEnumerable<string> Search(string location)
    {
        var normalizedLocation = LocationNormalizer.Normalize(location);

        if (normalizedLocation is null)
        {
            _logger.LogWarning("Invalid location format: {Location}", location);
            return Enumerable.Empty<string>();
        }

        var node = _currentRoot.FindDeepest(normalizedLocation.Value.Segments);
        var platforms = node?.CumulativePlatforms ?? ImmutableSortedSet<string>.Empty;

        _logger.LogInformation("Platforms found for location {Location}: {Platforms}", location, string.Join(", ", platforms));

        return platforms;
    }
}