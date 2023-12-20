namespace OpenWFCsharp.Messages.Dls;

/// <summary>
/// Request data to the 'dls1' server for the 'list' action.
/// </summary>
public record DlsListRequest
{
    private readonly Dictionary<string, string?> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DlsListRequest"/> class.
    /// </summary>
    /// <param name="parameters">Request data.</param>
    public DlsListRequest(Dictionary<string, string?> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        this.parameters = parameters;
    }

    /// <summary>
    /// Gets or sets the start index of files to return in the list.
    /// </summary>
    public int Offset {
        get => parameters.ContainsKey("offset") ? int.Parse(parameters["offset"]!) : 0;
        set => parameters["offset"] = value.ToString();
    }

    /// <summary>
    /// Gets or sets the number of file information to return in the list.
    /// </summary>
    public int Number {
        get => parameters.ContainsKey("num") ? int.Parse(parameters["num"]!) : 0;
        set => parameters["num"] = value.ToString();
    }
}
