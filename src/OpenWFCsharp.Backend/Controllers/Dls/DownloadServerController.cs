namespace OpenWFCsharp.Backend.Controllers.Dls;

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using OpenWFCsharp.Backend.Controllers.Dls.Storage;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        string serviceHost = Request.Headers.Host.ToString();
        serviceHost = string.IsNullOrEmpty(serviceHost) ? "localhost" : serviceHost;
        Response.Headers.Append("X-DLS-Host", serviceHost);
        Response.Headers.Append("Date", DateTime.UtcNow.ToString("r"));
        Response.Headers.Append("Server", "OpenWFCsharp");

        // TODO: validate token for the user.
        // TODO: validate game and password
        if (string.IsNullOrEmpty(data.Action)) {
            return BadRequest("Missing action parameter");
        }

        return data.Action switch {
            "count" => ProcessCount(data),
            "list" => ProcessList(data),
            "contents" => ProcessContents(data),
            _ => NotFound($"Unknown action: '{data.Action}'"),
        };
    }

    private IActionResult ProcessCount(DlsRequest data)
    {
        if (string.IsNullOrEmpty(data.GameCode)) {
            return BadRequest("Missing gamecode");
        }

        string[] attributes = [
            data.FileAttribute1 ?? string.Empty,
            data.FileAttribute2 ?? string.Empty,
            data.FileAttribute3 ?? string.Empty,
        ];

        int count = storage.CountFiles(data.GameCode, attributes);
        return Content(count.ToString());
    }

    private IActionResult ProcessList(DlsRequest data)
    {
        if (string.IsNullOrEmpty(data.GameCode)) {
            return BadRequest("Missing gamecode");
        }

        string[] requestAttributes = [
            data.FileAttribute1 ?? string.Empty,
            data.FileAttribute2 ?? string.Empty,
            data.FileAttribute3 ?? string.Empty,
        ];

        IEnumerable<GameFileInfo> files = storage.GetList(data.GameCode, requestAttributes)
            .Skip(data.List.Offset)
            .Take(data.List.Number);

        StringBuilder output = new();
        foreach (GameFileInfo file in files) {
            string[] fileAttributes = new string[3];
            if (file.Attributes is not null) {
                fileAttributes[0] = (file.Attributes.Length > 0) ? (file.Attributes[0] ?? "") : "";
                fileAttributes[1] = (file.Attributes.Length > 1) ? (file.Attributes[1] ?? "") : "";
                fileAttributes[2] = (file.Attributes.Length > 2) ? (file.Attributes[2] ?? "") : "";
            }

            _ = output.Append(file.Name).Append('\t')
                .Append(fileAttributes[0]).Append('\t')
                .Append(fileAttributes[1]).Append('\t')
                .Append(fileAttributes[2]).Append('\t')
                .Append('\t')
                .Append(file.FileLength)
                .Append("\r\n");
        }

        return Content(output.ToString());
    }

    private IActionResult ProcessContents(DlsRequest data)
    {
        if (string.IsNullOrEmpty(data.GameCode)) {
            return BadRequest("Missing gamecode");
        } else if (string.IsNullOrEmpty(data.Contents.Name)) {
            return BadRequest("Missing file to download");
        }

        Stream file;
        try {
            file = storage.GetFile(data.GameCode, data.Contents.Name);
        } catch {
            return NotFound("File not found");
        }

        return File(file, "application/x-dsdl", data.Contents.Name);
    }
}
