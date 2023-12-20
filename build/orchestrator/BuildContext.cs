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

        DockerProjects = new();
    }

    public Collection<OpenApiProjectInfo> OpenApiProjects { get; }

    public string OpenApiDocFxPath { get; set; }

    public Collection<DotnetDockerProjectInfo> DockerProjects { get; }
}
