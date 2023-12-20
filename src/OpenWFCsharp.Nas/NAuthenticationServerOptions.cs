namespace OpenWFCsharp.Nas;

/// <summary>
/// Options for the 'nas' server.
/// </summary>
public class NAuthenticationServerOptions
{
    /// <summary>
    /// Key name in the configuration file for this option.
    /// </summary>
    public static string OptionName => "NAuthServer";

    /// <summary>
    /// Gets or sets a value indicating whether the service should validate
    /// the user's game passwords.
    /// </summary>
    public bool ValidatePasswords { get; set; }

    /// <summary>
    /// Gets or sets the collection of service IDs and their hostname.
    /// </summary>
    public IReadOnlyDictionary<int, string> Services { get; set; } = new Dictionary<int, string>();
}
