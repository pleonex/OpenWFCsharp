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
        SvcLoc = new NasSvcLocRequest(Parameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NasRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public NasRequest(Dictionary<string, string?> parameters)
        : base(parameters)
    {
        SvcLoc = new NasSvcLocRequest(parameters);
    }

    /// <summary>
    /// Gets the action request to the server.
    /// </summary>
    public required string? Action {
        get => Parameters.GetValueOrDefault("action");
        init => Parameters["action"] = value;
    }

    /// <summary>
    /// Gets the request data for the 'SVCLOC' action.
    /// </summary>
    public NasSvcLocRequest SvcLoc { get; }
}
