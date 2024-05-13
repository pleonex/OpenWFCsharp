namespace OpenWFCsharp.Messages.Nas;

/// <summary>
/// Response from the authentication 'nas' server for the create account request.
/// </summary>
public record NasAcctCreateResponse : NasResponse
{
    /// <summary>
    /// Gets or sets the user ID after registration.
    /// </summary>
    public string UserId {
        get => Parameters["userid"];
        init => Parameters["userid"] = value;
    }
}
