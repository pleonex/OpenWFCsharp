namespace BuildSystem;

using System.Text;
using Cake.Common;
using Cake.Common.IO;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Publish;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("BuildDockerImage")]
public class BuildDockerImageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetPublish(
            context.DockerWebProject,
            new DotNetPublishSettings {
                Configuration = context.DotNetContext.Configuration,
                OS = "linux",
                ArgumentCustomization = b => b
                    .AppendQuoted("--arch").AppendQuoted("x64")
                    .AppendQuoted("-p:PublishProfile=DefaultContainer")
                    .Append("-p").Append($"ContainerImageTags={context.Version}"),
            });

        // Export into tar
        context.Log.Information("Exporting image");
        string projectName = Path.GetFileName(context.DockerWebProject);
        string dockerFilename = $"docker_{projectName}-{context.Version}.tar";
        string dockerImageTempPath = Path.Combine(context.TemporaryPath, dockerFilename);
        string dockerArgs = new StringBuilder()
            .Append("save")
            .Append(" -o").Append(dockerImageTempPath)
            .Append($" {context.DockerImageName}:{context.Version}")
            .ToString();
        int retCode = context.StartProcess("docker", dockerArgs);
        if (retCode != 0) {
            throw new Exception("Docker save failed!");
        }

        // Zip
        string dockerImagePath = Path.Combine(context.ArtifactsPath, $"{dockerFilename}.zip");
        context.Zip(context.TemporaryPath, dockerImagePath, new string[] { dockerImageTempPath });
        context.DeliveriesContext.BinaryFiles.Add(dockerImagePath);
    }
}
