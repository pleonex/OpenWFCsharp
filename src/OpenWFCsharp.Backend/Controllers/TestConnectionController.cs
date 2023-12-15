namespace OpenWFCsharp.Backend.Controllers;

using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Test connection controller.
/// </summary>
[Route("")]
[ApiController]
public class TestConnectionController : ControllerBase
{
    private const string DevVersion = "0.0.0-dev";
    private static readonly string AppVersion = GetAppVersion();

    /// <summary>
    /// HTTP GET to check Internet connection.
    /// </summary>
    /// <returns>200 OK.</returns>
    [HttpGet]
    public IActionResult Ping()
    {
        Response.Headers.Append("X-Organization", "PleOps");
        Response.Headers.Append("Server", "OpenWFCSharp");
        return Ok($"Hello from OpenWFCsharp {AppVersion} by pleonex");
    }

    private static string GetAppVersion()
    {
        string assemblyPath = Assembly.GetExecutingAssembly().Location;
        string version = FileVersionInfo.GetVersionInfo(assemblyPath).ProductVersion
            ?? DevVersion;
        if (version.Contains('+')) {
            version = version[..version.IndexOf('+')];
        }

        return (version == DevVersion) ? "DEVELOPMENT BUILD" : $"v{version}";
    }
}
