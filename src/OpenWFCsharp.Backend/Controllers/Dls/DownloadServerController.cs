﻿namespace OpenWFCsharp.Backend.Controllers.Dls;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint that provides game download content ('dls1' server).
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
        return Content("0");
    }

    private IActionResult ProcessList()
    {
        return Content("");
    }

    private IActionResult ProcessContents()
    {
        return Content("");
    }
}
