namespace OpenWFCsharp.Messages.Nas;

/// <summary>
/// Request data to the 'nas' server for the 'login' action.
/// </summary>
public record NasLoginRequest
{
    private readonly Dictionary<string, string?> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="NasLoginRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public NasLoginRequest(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        this.parameters = parameters;
    }

    /// <summary>
    /// Gets the unknown 'gsbr' code.
    /// </summary>
    public string? GsbrCode {
        get => parameters.GetValueOrDefault("gsbrcd");
        set => parameters["gsbrcd"] = value;
    }
}
