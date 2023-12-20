namespace OpenWFCsharp.Messages.Nas;

/// <summary>
/// Request data to the 'nas' server for the 'SVCLOC' action.
/// </summary>
public record NasSvcLocRequest
{
    private readonly Dictionary<string, string?> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="NasSvcLocRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public NasSvcLocRequest(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        this.parameters = parameters;
    }

    /// <summary>
    /// Gets or sets the requested service code.
    /// </summary>
    public int Service {
        get => parameters.ContainsKey("svc") ? int.Parse(parameters["svc"]!) : -1;
        set => parameters["svc"] = value.ToString();
    }
}
