namespace BuildSystem;

using System.Collections.ObjectModel;
using System.Text.Json;
using Cake.Core;
using Cake.Frosting.PleOps.Recipe;

public class BuildContext : PleOpsBuildContext
{
    private string infoPath;

    public BuildContext(ICakeContext context)
        : base(context)
    {
        infoPath = string.Empty;
        OpenApiProjects = new();
        OpenApiDocFxPath = "docs/rest/";

        DockerRegistry = string.Empty;
        DockerProjects = new();
        DockerImageArtifacts = new();

        ResourcesToBundle = new();
    }

    public Collection<OpenApiProjectInfo> OpenApiProjects { get; }

    public string OpenApiDocFxPath { get; set; }

    public Collection<DotnetDockerProjectInfo> DockerProjects { get; }

    public string DockerRegistry { get; set; }

    public Collection<string> DockerImageArtifacts { get; private set; }

    public Collection<string> ResourcesToBundle { get; }

    public override void ReadArguments()
    {
        base.ReadArguments();

        infoPath = Path.Combine(ArtifactsPath, "extended.artifacts.json");

        if (File.Exists(infoPath)) {
            string json = File.ReadAllText(infoPath);
            var actual = JsonSerializer.Deserialize<Collection<string>>(json)
                ?? throw new FormatException("Cannot deserialize deliveries info");

            DockerImageArtifacts = actual;
        }
    }

    public void Save()
    {
        string? outDir = Path.GetDirectoryName(infoPath);
        if (!string.IsNullOrEmpty(outDir) && !Directory.Exists(outDir)) {
            _ = Directory.CreateDirectory(outDir);
        }

        string json = JsonSerializer.Serialize(DockerImageArtifacts);
        File.WriteAllText(infoPath, json);
    }
}
