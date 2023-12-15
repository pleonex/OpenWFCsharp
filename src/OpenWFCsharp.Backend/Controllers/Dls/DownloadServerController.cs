namespace OpenWFCsharp.Backend.Controllers.Dls;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for the 'DLS1' server.
/// </summary>
[Route("download")]
[ApiController]
public class DownloadServerController : ControllerBase
{
    private readonly ILogger<DownloadServerController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DownloadServerController"/> class.
    /// </summary>
    /// <param name="logger">Logger for the instance.</param>
    public DownloadServerController(ILogger<DownloadServerController> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        this.logger = logger;
    }

    /// <summary>
    /// Post a request to the DLS server.
    /// </summary>
    /// <param name="data">URL-NBase64 encoded data with the request.</param>
    /// <returns>Operation result.</returns>
    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult PostRequest([FromForm] Dictionary<string, string> data)
    {
        data = Base64UrlEncodedContent.Decode(data);
        logger.LogDebug("Request parameters: {data}", data);

        if (!data.TryGetValue("action", out string? action)) {
            return BadRequest("Missing action parameter");
        }

        Response.Headers.Append("X-DLS-Host", "pleonex.dev"); // TODO: from config
        Response.Headers.Append("Date", DateTime.UtcNow.ToString("r"));
        Response.Headers.Append("Server", "OpenWFCsharp");

        return action switch {
            "count" => ProcessCount(),
            "list" => ProcessList(),
            "contents" => ProcessContents(),
            _ => NotFound($"Unknown action: '{action}'"),
        };
    }

    private IActionResult ProcessCount()
    {
        return Ok("0");
    }

    private IActionResult ProcessList()
    {
        return Ok();
    }

    private IActionResult ProcessContents()
    {
        return Ok();
    }
}
