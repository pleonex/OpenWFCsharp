namespace OpenWFCsharp.Backend.Controllers.Dls;

using System.Globalization;

/// <summary>
/// Requests to the download content 'dls1' server.
/// </summary>
public record DlsRequest : DwcRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DlsRequest"/> class.
    /// </summary>
    public DlsRequest()
        : base()
    {
        List = new DlsListRequest(Parameters);
        Contents = new DlsContentsRequest(Parameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DlsRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public DlsRequest(Dictionary<string, string?> parameters)
        : base(parameters)
    {
        List = new DlsListRequest(parameters);
        Contents = new DlsContentsRequest(parameters);
    }

    /// <summary>
    /// Gets the action request to the server.
    /// </summary>
    public string? Action {
        get => Parameters.GetValueOrDefault("action");
        init => Parameters["action"] = value;
    }

    /// <summary>
    /// Gets the code of the game.
    /// </summary>
    public string? GameCode {
        get => Parameters.GetValueOrDefault("gamecd");
        init => Parameters["gamecd"] = value;
    }

    /// <summary>
    /// Gets the code of the game as it appears in the header of the ROM.
    /// </summary>
    public string? RomHeaderGameCode {
        get => Parameters.GetValueOrDefault("rhgamecd");
        init => Parameters["rhgamecd"] = value;
    }

    /// <summary>
    /// Gets the access password for the given game.
    /// </summary>
    public string? Password {
        get => Parameters.GetValueOrDefault("passwd");
        init => Parameters["passwd"] = value;
    }

    /// <summary>
    /// Gets the user session token for this service.
    /// </summary>
    public string? Token {
        get => Parameters.GetValueOrDefault("token");
        init => Parameters["token"] = value;
    }

    /// <summary>
    /// Gets or sets the ID of the user.
    /// </summary>
    public long UserId {
        get => Parameters.ContainsKey("userid") ? long.Parse(Parameters["userid"]!) : -1;
        init => Parameters["userid"] = value.ToString();
    }

    /// <summary>
    /// Gets or sets the device MAC address.
    /// </summary>
    public long MacAddress {
        get => Parameters.ContainsKey("macadr")
            ? long.Parse(Parameters["macadr"]!, NumberStyles.HexNumber)
            : -1;
        init => Parameters["macadr"] = value.ToString("x12");
    }

    /// <summary>
    /// Gets or sets the information from the user Wi-Fi access-point.
    /// </summary>
    public string? AccessPointInfo {
        get => Parameters.GetValueOrDefault("apinfo");
        init => Parameters["apinfo"] = value;
    }

    /// <summary>
    /// Gets the optional file attribute for filtering.
    /// </summary>
    public string? FileAttribute1 {
        get => Parameters.GetValueOrDefault("attr1");
        init => Parameters["attr1"] = value;
    }

    /// <summary>
    /// Gets the optional file attribute for filtering.
    /// </summary>
    public string? FileAttribute2 {
        get => Parameters.GetValueOrDefault("attr2");
        init => Parameters["attr2"] = value;
    }

    /// <summary>
    /// Gets the optional file attribute for filtering.
    /// </summary>
    public string? FileAttribute3 {
        get => Parameters.GetValueOrDefault("attr3");
        init => Parameters["attr3"] = value;
    }

    /// <summary>
    /// Request data for the 'list' action.
    /// </summary>
    public DlsListRequest List { get; }

    /// <summary>
    /// Request data for the 'contents' action.
    /// </summary>
    public DlsContentsRequest Contents { get; }
}
