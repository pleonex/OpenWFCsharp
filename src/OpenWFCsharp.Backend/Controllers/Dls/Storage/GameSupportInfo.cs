namespace OpenWFCsharp.Backend.Controllers.Dls.Storage;

using System.Collections.ObjectModel;

/// <summary>
/// Information about a game that the DLS server supports.
/// </summary>
public record GameSupportInfo
{
    /// <summary>
    /// Gets or sets the game code.
    /// </summary>
    public required string GameCode { get; init; }

    /// <summary>
    /// Gets or sets the password for the DLS server.
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// Gets or sets relative path in the storage to the files.
    /// </summary>
    public required string StoragePath { get; set; }

    /// <summary>
    /// Gets or sets the information about the files to download from the game.
    /// </summary>
    public required Collection<GameFileInfo> Files { get; init; }
}
