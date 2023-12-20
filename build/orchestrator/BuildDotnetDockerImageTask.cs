namespace BuildSystem;

using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("BuildDotnetDockerImage")]
public class BuildDotnetDockerImageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (DotnetDockerProjectInfo info in context.DockerProjects) {
            BuildProject(context, info);
        }
    }

    private void BuildProject(BuildContext context, DotnetDockerProjectInfo info)
    {
        context.Log.Information("Creating Docker image from: {0}", info.ProjectPath);

        context.DotNetPublish(
            info.ProjectPath,
            new DotNetPublishSettings {
                Configuration = context.DotNetContext.Configuration,
                OS = "linux",
                ArgumentCustomization = b => b
                    .AppendQuoted("--arch").AppendQuoted(info.Architecture)
                    .AppendQuoted("-p:PublishProfile=DefaultContainer")
                    .Append("-p").Append($"ContainerImageTags={context.Version}")
                    .Append("-p").AppendQuoted($"Version={context.Version}"),
            });
    }
}
