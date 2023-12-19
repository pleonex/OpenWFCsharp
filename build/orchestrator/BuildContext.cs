namespace BuildSystem;

using Cake.Core;
using Cake.Frosting.PleOps.Recipe;

public class BuildContext : PleOpsBuildContext
{
    public BuildContext(ICakeContext context)
        : base(context)
    {
        SwaggerDocName = string.Empty;
        SwaggerProjectPath = string.Empty;
        SwaggerAssemblyPath = string.Empty;
        SwaggerDocFxPath = "docs/rest/";

        DockerWebProject = string.Empty;
        DockerImageName = string.Empty;
    }

    public string SwaggerDocName { get; set; }

    public string SwaggerProjectPath { get; set; }

    public string SwaggerAssemblyPath { get; set; }

    public string SwaggerDocFxPath { get; set; }

    public string DockerWebProject { get; set; }

    public string DockerImageName { get; set; }
}
