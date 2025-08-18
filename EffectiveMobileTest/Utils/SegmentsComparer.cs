public sealed class SegmentsComparer : IEqualityComparer<string[]>
{
    private static readonly StringComparer Comparer = StringComparer.OrdinalIgnoreCase;

    public bool Equals(string[]? x, string[]? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null || x.Length != y.Length) return false;
        for (int i = 0; i < x.Length; i++)
            if (!Comparer.Equals(x[i], y[i])) return false;
        return true;
    }

    public int GetHashCode(string[] obj)
    {
        unchecked
        {
            int hash = 17;
            foreach (var s in obj)
                hash = hash * 31 + Comparer.GetHashCode(s);
            return hash;
        }
    }
}