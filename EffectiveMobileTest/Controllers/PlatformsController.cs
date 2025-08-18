using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController(IPlatformsService platformsService, ILogger<PlatformsController> logger) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload()
    {
        logger.LogInformation("Received request to upload platforms.");

        if (!Request.ContentType?.StartsWith("text/plain", StringComparison.OrdinalIgnoreCase) ?? true)
        {
            logger.LogWarning("Invalid Content-Type: {ContentType}", Request.ContentType);
            return BadRequest("Content-Type must be text/plain");
        }

        using var reader = new StreamReader(Request.Body);
        string body = await reader.ReadToEndAsync();
        var report = platformsService.LoadFromText(body);

        logger.LogInformation("Upload completed. Lines processed: {LinesProcessed}, Lines ignored: {LinesIgnored}, Total platforms: {TotalPlatforms}", report.LinesProcessed, report.LinesIgnored, report.TotalPlatforms);

        return Ok(report);
    }

    [HttpGet]
    public IActionResult Search([FromQuery] string location)
    {
        logger.LogInformation("Searching platforms for location: {Location}", location);

        var result = platformsService.Search(location);

        logger.LogInformation("Found {Count} platforms", result.Count());

        return Ok(result);
    }
}