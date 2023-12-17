namespace OpenWFCsharp.Backend.Controllers.Dls;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using OpenWFCsharp.Backend.Controllers.Dls.Storage;

/// <summary>
/// Endpoint that provides game download content ('dls1' server).
/// </summary>
[Route("download")]
[ApiController]
public class DownloadServerController(
    ILogger<DownloadServerController> logger,
    IContentStorage storage)
    : ControllerBase
{
    /// <summary>
    /// Sends an action requeseet to the download service.
    /// </summary>
    /// <param name="data">URL-NBase64 encoded data with the request.</param>
    /// <returns>Operation result.</returns>
    /// <response code="200">Request was accepted and processed.</response>
    /// <response code="400">Request missing 'action' parameter or missing 'host' header.</response>
    /// <response code="404">Unknown action request value.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PostDlsRequest([FromBody] DlsRequest data)
    {
        logger.LogDebug("Request parameters: {data}", data);

        string? serviceHost = Request.Headers.Host;
        if (serviceHost is null) {
            return BadRequest("Cannot determine myself. Please provide the 'host' header");
        }

        Response.Headers.Append("X-DLS-Host", serviceHost);
        Response.Headers.Append("Date", DateTime.UtcNow.ToString("r"));
        Response.Headers.Append("Server", "OpenWFCsharp");

        // TODO: validate token for the user.
        // TODO: validate game and password
        if (string.IsNullOrEmpty(data.Action)) {
            return BadRequest("Missing action parameter");
        }

        return data.Action switch {
            "count" => ProcessCount(),
            "list" => ProcessList(),
            "contents" => ProcessContents(),
            _ => NotFound($"Unknown action: '{data.Action}'"),
        };
    }

    private IActionResult ProcessCount()
    {
        int count = storage.CountFiles("VPYP", Array.Empty<string>());
        return Content(count.ToString());
    }

    private IActionResult ProcessList()
    {
        var list = storage.GetList("VPYP", Array.Empty<string>());
        return Content(string.Join(",", list.Select(l => l.Filename)));
    }

    private IActionResult ProcessContents()
    {
        return Content("");
    }
}
