namespace OpenWFCsharp.Backend.Controllers;

using System.Diagnostics;
using System.Net.Mime;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint that allows to test service availability.
/// </summary>
[Route("")]
[ApiController]
public class TestConnectionController : ControllerBase
{
    private const string DevVersion = "0.0.0-dev";
    private static readonly string HtmlTestPage = GetTestPage();

    /// <summary>
    /// Ping to check service availability and Internet connection.
    /// </summary>
    /// <returns>200 OK.</returns>
    /// <response code="200">Server available, returns test HTML page.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Ping()
    {
        Response.Headers.Append("X-Organization", "PleOps");
        Response.Headers.Append("Server", "OpenWFCSharp");
        return Content(HtmlTestPage, MediaTypeNames.Text.Html);
    }

    private static string GetTestPage()
    {
        // This was adapted from the original test page in 2023
        return @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html>
    
<head>
		<title>HTML Page</title>
</head>

<body bgcolor=""#FFFFFF"">
<!-- This is test.html page -->
Hello from OpenWFCsharp {AppVersion} by pleonex
</body>
    
</html>
"
        .Replace("{AppVersion}", GetAppVersion());
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
