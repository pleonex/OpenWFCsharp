namespace OpenWFCsharp.Backend.Controllers.Nas;

/// <summary>
/// Requests to the authorization 'nas' server.
/// </summary>
public record NasRequest : DwcRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NasRequest"/> class.
    /// </summary>
    public NasRequest()
        : base()
    {
        UserInfo = new NasUserInfo(Parameters);
        Login = new NasLoginRequest(Parameters);
        ServiceLocation = new NasSvcLocRequest(Parameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NasRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public NasRequest(Dictionary<string, string?> parameters)
        : base(parameters)
    {
        UserInfo = new NasUserInfo(parameters);
        Login = new NasLoginRequest(parameters);
        ServiceLocation = new NasSvcLocRequest(parameters);
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
    /// Gets the maker code of the game.
    /// </summary>
    public string? MakerCode {
        get => Parameters.GetValueOrDefault("makercd");
        init => Parameters["makercd"] = value;
    }

    /// <summary>
    /// Gets the access password for the given game.
    /// </summary>
    public string? Password {
        get => Parameters.GetValueOrDefault("passwd");
        init => Parameters["passwd"] = value;
    }

    /// <summary>
    /// Gets the information from the user making the request.
    /// </summary>
    public NasUserInfo UserInfo { get; }

    /// <summary>
    /// Gets the request data for the 'login' action.
    /// </summary>
    public NasLoginRequest Login { get; }

    /// <summary>
    /// Gets the request data for the 'SVCLOC' action.
    /// </summary>
    public NasSvcLocRequest ServiceLocation { get; }
}
