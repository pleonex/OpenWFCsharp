namespace OpenWFCsharp.Backend.Controllers.Dls.Storage;

using System.Collections.Generic;
using System.IO;

/// <summary>
/// Content storage based on folders on disk and YAML information.
/// </summary>
public class YamlContentStorage : IContentStorage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="YamlContentStorage"/> class.
    /// </summary>
    public YamlContentStorage()
    {
        // TODO: populate DB.
    }

    /// <inheritdoc />
    public int CountFiles(string gameCode, string[] attributes)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public Stream GetFile(string gameCode, string filename)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IEnumerable<GameFileInfo> GetList(string gameCode, string[] attributes)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool ValidateGameInfo(string gameCode, string password)
    {
        throw new NotImplementedException();
    }
}
