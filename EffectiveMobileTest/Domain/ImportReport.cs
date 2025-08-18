public sealed record ImportReport
{
    public int LinesProcessed { get; set; }
    public int LinesIgnored { get; set; }
    public int TotalPlatforms { get; set; }
    public string[] Notes { get; set; } = Array.Empty<string>();
}