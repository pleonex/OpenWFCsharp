namespace OpenWFCsharp.Backend.Controllers.Nas;

using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenWFCsharp.Backend.Security;

/// <summary>
/// Endpoint with simple logic from the 'nas' server.
/// </summary>
[Route("ac")]
[ApiController]
public class NAuthenticationServerController : ControllerBase
{
    private readonly ILogger<NAuthenticationServerController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NAuthenticationServerController"/> class.
    /// </summary>
    /// <param name="logger">Logger for the instance.</param>
    public NAuthenticationServerController(ILogger<NAuthenticationServerController> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        this.logger = logger;
    }

    /// <summary>
    /// Post a request to the NAS server.
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

        return action switch {
            "login" => ProcessLogin(),
            _ => NotFound($"Unknown action: '{action}'"),
        };
    }

    private IActionResult ProcessLogin()
    {
        // TODO: check if the game is allowed (game ID & maker & password)
        // TODO: check banned users by userid / bssid / apinfo / macaddr
        // TODO: register user in DB by storing token, challenge and user info
        var responseData = new Dictionary<string, string> {
            ["retry"] = "0",
            ["returncd"] = "001",
            ["locator"] = "gamespy.com",
            ["challenge"] = TokenService.GenerateChallenge(),
            ["token"] = TokenService.GenerateAuthToken(),
            ["datetime"] = DateTime.UtcNow.ToString("yyyyMMddHHmmss")
        };

        logger.LogDebug("Response parameters: {data}", responseData);

        Response.Headers.Append("NODE", "node1");
        Response.Headers.Append("Date", DateTime.UtcNow.ToString("r"));
        Response.Headers.Append("Server", "OpenWFCsharp (http)");
        return Content(Base64UrlEncodedContent.Encode(responseData), MediaTypeNames.Text.Plain);
    }
}
