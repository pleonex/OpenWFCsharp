namespace OpenWFCsharp.Backend.Controllers.Dls;

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
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DlsRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public DlsRequest(Dictionary<string, string?> parameters)
        : base(parameters)
    {
    }

    /// <summary>
    /// Gets the action request to the server.
    /// </summary>
    public required string? Action {
        get => Parameters.GetValueOrDefault("action");
        init => Parameters["action"] = value;
    }
}
