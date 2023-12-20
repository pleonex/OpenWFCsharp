namespace OpenWFCsharp.Dls.Storage;

using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;

/// <summary>
/// Content storage based on folders on disk and YAML information.
/// </summary>
public class YamlContentStorage : IContentStorage
{
    private readonly ILogger<YamlContentStorage> logger;
    private readonly List<GameSupportInfo> games;
    private readonly DownloadServerOptions serverOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="YamlContentStorage"/> class.
    /// </summary>
    public YamlContentStorage(
        ILogger<YamlContentStorage> logger,
        IOptions<DownloadServerOptions> serverOptions)
    {
        this.logger = logger;
        this.serverOptions = serverOptions.Value;

        games = [];
        PopulateDb();
    }

    /// <inheritdoc />
    public bool ValidateGameInfo(string gameCode, string password)
    {
        if (!serverOptions.ValidatePasswords) {
            return true;
        }

        GameSupportInfo? info = games.Find(i => i.GameCode == gameCode);
        return info is not null && info.Password == password;
    }

    /// <inheritdoc />
    public int CountFiles(string gameCode, string[] attributes)
    {
        GameSupportInfo? info = games.Find(i => i.GameCode == gameCode);
        if (info is null) {
            return 0;
        }

        return info.Files.Count(i => FileAttributeFilter(i, attributes));
    }

    /// <inheritdoc />
    public IEnumerable<GameFileInfo> GetList(string gameCode, string[] attributes)
    {
        GameSupportInfo? info = games.Find(i => i.GameCode == gameCode);
        if (info is null) {
            return Array.Empty<GameFileInfo>();
        }

        return info.Files.Where(i => FileAttributeFilter(i, attributes));
    }

    /// <inheritdoc />
    public Stream GetFile(string gameCode, string filename)
    {
        GameSupportInfo info = games.Find(i => i.GameCode == gameCode)
            ?? throw new FileNotFoundException("Missing game code");

        GameFileInfo fileInfo = info.Files.FirstOrDefault(i => i.Name == filename)
            ?? throw new FileNotFoundException("Missing requested file");

        string filePath = GetFilePath(info, fileInfo);
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    private static string GetFilePath(GameSupportInfo gameInfo, GameFileInfo fileInfo)
    {
        string name = string.IsNullOrEmpty(fileInfo.PhysicalFilename)
            ? fileInfo.Name
            : fileInfo.PhysicalFilename;
        string filePath = Path.Combine(gameInfo.StoragePath, name);
        if (!File.Exists(filePath)) {
            throw new FileNotFoundException("Missing downloadable file", filePath);
        }

        return filePath;
    }

    private static bool FileAttributeFilter(GameFileInfo info, string[]? requestAttributes)
    {
        if (requestAttributes is null) {
            return true;
        }

        bool result = true;
        if (requestAttributes.Length > 0) {
            result &= (info.Attributes is { Length: > 0 }) && (info.Attributes[0] == requestAttributes[0]);
        }

        if (requestAttributes.Length > 1 && !string.IsNullOrEmpty(requestAttributes[1])) {
            result &= (info.Attributes is { Length: > 1 }) && (info.Attributes[1] == requestAttributes[1]);
        }

        if (requestAttributes.Length > 2 && !string.IsNullOrEmpty(requestAttributes[2])) {
            result &= (info.Attributes is { Length: > 2 }) && (info.Attributes[2] == requestAttributes[2]);
        }

        return result;
    }

    private void PopulateDb()
    {
        IEnumerable<string> files;
        try {
            string content = File.ReadAllText(serverOptions.StorageDB);
            files = new DeserializerBuilder()
                .Build()
                .Deserialize<IEnumerable<string>>(content);
        } catch (Exception ex) {
            logger.LogCritical(ex, "Cannot read storage database");
            throw;
        }

        string dbDirectory = Path.GetDirectoryName(serverOptions.StorageDB)!;
        foreach (string relativePath in files) {
            string yamlFile = Path.Combine(dbDirectory, relativePath);
            ReadGameInfo(yamlFile);
        }
    }

    private void ReadGameInfo(string gameInfoPath)
    {
        IEnumerable<GameSupportInfo> definitions;
        try {
            string content = File.ReadAllText(gameInfoPath);
            definitions = new DeserializerBuilder()
                .Build()
                .Deserialize<IEnumerable<GameSupportInfo>>(content);

            games.AddRange(definitions);
        } catch (Exception ex) {
            logger.LogCritical(ex, "Cannot read storage game definition: {file}", gameInfoPath);
            throw;
        }

        string dbDirectory = Path.GetDirectoryName(serverOptions.StorageDB)!;
        foreach (GameSupportInfo gameInfo in definitions) {
            gameInfo.StoragePath = Path.Combine(dbDirectory, gameInfo.StoragePath);

            var allFiles = gameInfo.Files.ToList();
            foreach (GameFileInfo fileInfo in allFiles) {
                try {
                    string filePath = GetFilePath(gameInfo, fileInfo);
                    using Stream temp = File.OpenRead(filePath);
                    fileInfo.FileLength = temp.Length;
                } catch (FileNotFoundException ex) {
                    logger.LogError("Removing missing downloadable file {path} {info}", ex.FileName, fileInfo);
                    gameInfo.Files.Remove(fileInfo);
                }
            }
        }
    }
}
