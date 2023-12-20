namespace OpenWFCsharp.Messages.Dls;

/// <summary>
/// Request data to the 'dls1' server for the 'contents' action.
/// </summary>
public record DlsContentsRequest
{
    private readonly Dictionary<string, string?> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DlsContentsRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public DlsContentsRequest(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        this.parameters = parameters;
    }

    /// <summary>
    /// Gets or sets the name of the file to provide.
    /// </summary>
    public string? Name {
        get => parameters.GetValueOrDefault("contents");
        set => parameters["contents"] = value;
    }
}
