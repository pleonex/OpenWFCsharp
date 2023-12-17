namespace OpenWFCsharp.Backend.Controllers.Dls.Storage;

/// <summary>
/// Information about a downloadable game file from the server.
/// </summary>
/// <param name="Name">Name of the file to return.</param>
/// <param name="PhysicalFilename">Name of the file on disk. If not set, same as the file name.</param>
/// <param name="FileLength">Length of the file.</param>
/// <param name="Attributes">Optional file attributes for filtering.</param>
public record GameFileInfo(
    string Name,
    string? PhysicalFilename,
    long FileLength,
    string[]? Attributes);
