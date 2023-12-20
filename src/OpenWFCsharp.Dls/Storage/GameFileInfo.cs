namespace OpenWFCsharp.Dls.Storage;

/// <summary>
/// Information about a downloadable game file from the server.
/// </summary>
public record GameFileInfo
{
    /// <summary>
    /// Gets the name of the file to return.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the name of the file on disk. If not set, same as the file name.
    /// </summary>
    public string? PhysicalFilename { get; init; }

    /// <summary>
    /// Gets the length of the file.
    /// </summary>
    public long FileLength { get; set; }

    /// <summary>
    /// Gets the optional file attributes for filtering.
    /// </summary>
    public string[]? Attributes { get; init; }
}
