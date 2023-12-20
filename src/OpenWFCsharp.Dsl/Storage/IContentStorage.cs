namespace OpenWFCsharp.Dls.Storage;

/// <summary>
/// Interface to access the content storage for game bonus content.
/// </summary>
public interface IContentStorage
{
    /// <summary>
    /// Validates the game information of a supported game.
    /// </summary>
    /// <param name="gameCode">Game code.</param>
    /// <param name="password">Password for the DLS server for that game.</param>
    /// <returns>Value indicating whether the server supports the game and it provides valid information.</returns>
    bool ValidateGameInfo(string gameCode, string password);

    /// <summary>
    /// Get the number of files available that matches the query parameters.
    /// </summary>
    /// <param name="gameCode">Game code.</param>
    /// <param name="attributes">Optional attributes to use as filters.</param>
    /// <returns>Number of available files.</returns>
    int CountFiles(string gameCode, string[] attributes);

    /// <summary>
    /// Get the list of files that maches the query parameters.
    /// </summary>
    /// <param name="gameCode">Game code.</param>
    /// <param name="attributes">Optional attributes to use as filters.</param>
    /// <returns>List of file information.</returns>
    IEnumerable<GameFileInfo> GetList(string gameCode, string[] attributes);

    /// <summary>
    /// Open the requested file.
    /// </summary>
    /// <param name="gameCode">Game code.</param>
    /// <param name="filename">File name to retrieve.</param>
    /// <returns>Binary file data.</returns>
    Stream GetFile(string gameCode, string filename);
}
