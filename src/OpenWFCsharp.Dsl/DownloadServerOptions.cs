namespace OpenWFCsharp.Dls;

/// <summary>
/// Options for the download server 'dls'.
/// </summary>
public class DownloadServerOptions
{
    /// <summary>
    /// Key name in the configuration file for this option.
    /// </summary>
    public static string OptionName => "DownloadServer";

    /// <summary>
    /// Gets or sets a value indicating whether the service should validate
    /// the user's game passwords.
    /// </summary>
    public bool ValidatePasswords { get; set; }

    /// <summary>
    /// Gets or sets the path to the storage database file.
    /// </summary>
    public string StorageDB { get; set; } = string.Empty;
}
