namespace OpenWFCsharp.Backend.Controllers.Nas;

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
    public string? Service {
        get => parameters.GetValueOrDefault("svc");
        set => parameters["svc"] = value;
    }
}
