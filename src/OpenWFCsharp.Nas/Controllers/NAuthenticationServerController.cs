namespace OpenWFCsharp.Nas.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenWFCsharp.Messages.Nas;
using OpenWFCsharp.Nas.Identity;

/// <summary>
/// Endpoint that authenticates and authorizes users into the service ('nas' server).
/// </summary>
[Route("ac")]
[ApiController]
public class NAuthenticationServerController(
    ILogger<NAuthenticationServerController> logger,
    IOptions<NAuthenticationServerOptions> options)
    : ControllerBase
{
    /// <summary>
    /// Sends an action request to the auth service..
    /// </summary>
    /// <param name="data">URL-NBase64 encoded data with the request.</param>
    /// <returns>Operation result.</returns>
    /// <remarks>Supported actions are: acctcreate, login, SVCLOC.</remarks>
    /// <response code="200">Request processed, see output for the request success.</response>
    /// <response code="400">Request missing 'action' parameter or 'host' header.</response>
    /// <response code="404">Unknown action request value or unknown service location.</response>
    [HttpPost]
    [Consumes(MediaTypeNames.Application.FormUrlEncoded)]
    [Produces(MediaTypeNames.Text.Plain)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<NasResponse> PostNasRequest([FromBody] NasRequest data)
    {
        logger.LogDebug("Request parameters: {data}", data);

        Response.Headers.Append("NODE", "node1");
        Response.Headers.Append("Date", DateTime.UtcNow.ToString("r"));
        Response.Headers.Append("Server", "OpenWFCsharp (http)");

        if (string.IsNullOrEmpty(data.Action)) {
            return BadRequest("Missing action");
        }

        return data.Action switch {
            "acctcreate" => ProcessCreateAccount(data), // DSi only?
            "login" => ProcessLogin(),
            "SVCLOC" => ProcessSvcLoc(data),
            _ => NotFound($"Unknown action: '{data.Action}'"),
        };
    }

    private ActionResult<NasResponse> ProcessCreateAccount(NasRequest data)
    {
        // TODO: check banned users by userid / bssid / apinfo / macaddr
        // TODO: register user in DB storing user info and returning ID
        var response = new NasAcctCreateResponse {
            ReturnCode = (int)NasReturnCodes.AccountCreated,
            // must be 13-chars, same as input or from our DB?
            UserId = data.UserInfo.UserId.ToString().PadLeft(13, '0'),
        };

        logger.LogDebug("Response parameters: {data}", response);
        return response;
    }

    private ActionResult<NasResponse> ProcessLogin()
    {
        // TODO: check if the game is allowed (game ID & maker & password)
        // TODO: check banned users by userid / bssid / apinfo / macaddr
        // TODO: register user in DB by storing token, challenge and user info
        var response = new NasLoginResponse {
            ReturnCode = (int)NasReturnCodes.LoginSuccess,
            Locator = "openwfcsharp",
            Challenge = TokenService.GenerateChallenge(),
            Token = "NDS" + TokenService.GenerateAuthToken(),
            DateTime = DateTime.UtcNow,
        };

        logger.LogDebug("Response parameters: {data}", response);
        return response;
    }

    private ActionResult<NasResponse> ProcessSvcLoc(NasRequest request)
    {
        // TODO: verify user was registered in DB via login
        int serviceRequest = request.ServiceLocation.Service;
        if (serviceRequest == -1) {
            return BadRequest("Missing service");
        }

        var knownServices = options.Value.Services;
        if (!knownServices.TryGetValue(serviceRequest, out string? serviceHost)
            || string.IsNullOrEmpty(serviceHost)) {
            return NotFound($"Cannot find service: {serviceRequest}");
        }

        // TODO: register service token for the targeted service
        string serviceToken = TokenService.GenerateAuthToken();
        var response = new NasSvcLocResponse() {
            ReturnCode = (int)NasReturnCodes.Redirect,
            ServiceToken = serviceToken,
            Status = true,
            ServiceHost = serviceHost,
            DateTime = DateTime.UtcNow,
        };

        logger.LogDebug("Response parameters: {data}", response);
        return response;
    }
}
