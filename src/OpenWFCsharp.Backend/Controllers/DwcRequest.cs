namespace OpenWFCsharp.Backend.Controllers;

/// <summary>
/// Abstract type for requests to DWC servers.
/// </summary>
public abstract record DwcRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DwcRequest"/> class.
    /// </summary>
    protected DwcRequest()
    {
        Parameters = new();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DwcRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    protected DwcRequest(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        Parameters = parameters;
    }

    /// <summary>
    /// Gets the request data from the body as a dictionary.
    /// </summary>
    protected Dictionary<string, string?> Parameters { get; }
}
