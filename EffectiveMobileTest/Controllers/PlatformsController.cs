using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformsService _platformsService;
    private readonly ILogger<PlatformsController> _logger;

    public PlatformsController(IPlatformsService platformsService, ILogger<PlatformsController> logger)
    {
        _platformsService = platformsService;
        _logger = logger;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        _logger.LogInformation("Received request to upload platforms.");

        if (!Request.ContentType?.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase) ?? true)
        {
            _logger.LogWarning("Invalid Content-Type: {ContentType}", Request.ContentType);
            return BadRequest("Content-Type must be text/plain");
        }

        using var reader = new StreamReader(Request.Body);
        string body = await reader.ReadToEndAsync();
        var report = _platformsService.LoadFromText(body);

        _logger.LogInformation("Upload completed. Lines processed: {LinesProcessed}, Lines ignored: {LinesIgnored}, Total platforms: {TotalPlatforms}", report.LinesProcessed, report.LinesIgnored, report.TotalPlatforms);

        return Ok(report);
    }

    [HttpGet]
    public IActionResult Search([FromQuery] string location)
    {
        _logger.LogInformation("Searching platforms for location: {Location}", location);
        var result = _platformsService.Search(location);
        _logger.LogInformation("Found {Count} platforms", result.Count());
        return Ok(result);
    }
}