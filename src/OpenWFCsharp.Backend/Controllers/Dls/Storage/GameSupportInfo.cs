namespace OpenWFCsharp.Backend.Controllers.Dls.Storage;

using System.Collections.ObjectModel;

/// <summary>
/// Information about a game that the DLS server supports.
/// </summary>
/// <param name="GameCode">Game code.</param>
/// <param name="Password">Password for the DLS server.</param>
/// <param name="StoragePath">Relative path in the storage to the files.</param>
/// <param name="Files">Information about the files to download from the game.</param>
public record GameSupportInfo(
    string GameCode,
    string Password,
    string StoragePath,
    Collection<GameFileInfo> Files);
