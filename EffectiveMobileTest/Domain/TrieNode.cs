using System.Collections.Immutable;

public sealed class TrieNode
{
    public ImmutableDictionary<string, TrieNode> Children { get; private init; }
    public ImmutableHashSet<string> PlatformsHere { get; private init; }
    public ImmutableSortedSet<string> CumulativePlatforms { get; private init; }

    private TrieNode(
        ImmutableDictionary<string, TrieNode> children,
        ImmutableHashSet<string> platformsHere,
        ImmutableSortedSet<string> cumulative)
    {
        Children = children;
        PlatformsHere = platformsHere;
        CumulativePlatforms = cumulative;
    }

    public static TrieNode Empty { get; } = new(
        ImmutableDictionary<string, TrieNode>.Empty,
        ImmutableHashSet<string>.Empty,
        ImmutableSortedSet<string>.Empty
    );

    public static TrieNode Build(Dictionary<string[], HashSet<string>> map)
    {
        var root = new MutableNode();
        foreach (var kvp in map)
        {
            var segments = kvp.Key;
            var platforms = kvp.Value;
            var node = root;
            foreach (var segment in segments)
            {
                if (!node.Children.TryGetValue(segment, out var child))
                {
                    child = new MutableNode();
                    node.Children[segment] = child;
                }
                node = child;
            }
            foreach (var platform in platforms)
                node.PlatformsHere.Add(platform);
        }

        return Freeze(root, ImmutableSortedSet<string>.Empty);

        static TrieNode Freeze(MutableNode mutableNode, ImmutableSortedSet<string> parentCumulative)
        {
            var here = mutableNode.PlatformsHere.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
            var cumulative = parentCumulative
                .Union(here, StringComparer.OrdinalIgnoreCase)
                .ToImmutableSortedSet(StringComparer.OrdinalIgnoreCase);

            var childrenBuilder = ImmutableDictionary.CreateBuilder<string, TrieNode>(StringComparer.OrdinalIgnoreCase);
            foreach (var (segment, childMutable) in mutableNode.Children)
                childrenBuilder[segment] = Freeze(childMutable, cumulative);

            return new TrieNode(childrenBuilder.ToImmutable(), here, cumulative);
        }
    }

    public TrieNode? FindDeepest(IReadOnlyList<string> segments)
    {
        var node = this;
        foreach (var segment in segments)
        {
            if (!node.Children.TryGetValue(segment, out var next))
                return node;
            node = next;
        }
        return node;
    }

    private sealed class MutableNode
    {
        public Dictionary<string, MutableNode> Children { get; } = new(StringComparer.OrdinalIgnoreCase);
        public HashSet<string> PlatformsHere { get; } = new(StringComparer.OrdinalIgnoreCase);
    }
}