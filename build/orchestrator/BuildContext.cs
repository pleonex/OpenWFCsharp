namespace BuildSystem;

using System.Collections.ObjectModel;
using Cake.Core;
using Cake.Frosting.PleOps.Recipe;

public class BuildContext : PleOpsBuildContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
        OpenApiProjects = new();
        OpenApiDocFxPath = "docs/rest/";

        DockerWebProject = string.Empty;
        DockerImageName = string.Empty;
    }

    public Collection<OpenApiProjectInfo> OpenApiProjects { get; }

    public string OpenApiDocFxPath { get; set; }

    public string DockerWebProject { get; set; }

    public string DockerImageName { get; set; }
}
