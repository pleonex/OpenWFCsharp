namespace OpenWFCsharp.Messages.Nas;

/// <summary>
/// Error response from authentication 'nas' server.
/// </summary>
public record NasErrorResponse : NasResponse
{
    /// <summary>
    /// Gets or sets the reason for the error.
    /// </summary>
    public required string Reason {
        get => Parameters["reason"];
        init => Parameters["reason"] = value;
    }
}
