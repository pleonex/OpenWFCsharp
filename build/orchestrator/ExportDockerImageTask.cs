namespace BuildSystem;

using System.Text;
using Cake.Common;
using Cake.Common.IO;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("ExportDockerImage")]
public class ExportDockerImageTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (DotnetDockerProjectInfo info in context.DockerProjects) {
            ExportImage(context, info.ImageName);
        }
    }

    private void ExportImage(BuildContext context, string imageName)
    {
        context.Log.Information("Exporting Docker image: {0}", imageName);

        // Export into tar
        string projectName = imageName.Split('/')[^1];
        string dockerFilename = $"docker_{projectName}-{context.Version}.tar";
        string dockerImageTempPath = Path.Combine(context.TemporaryPath, dockerFilename);
        string dockerArgs = new StringBuilder()
            .Append("save")
            .Append(" -o").Append(dockerImageTempPath)
            .Append($" {imageName}:{context.Version}")
            .ToString();
        int retCode = context.StartProcess("docker", dockerArgs);
        if (retCode != 0) {
            throw new Exception("Docker save failed!");
        }

        // Zip
        try {
            string dockerImagePath = Path.Combine(context.ArtifactsPath, $"{dockerFilename}.zip");
            context.Zip(context.TemporaryPath, dockerImagePath, new string[] { dockerImageTempPath });
            context.DockerImageArtifacts.Add(dockerImagePath);
        } finally {
            File.Delete(dockerImageTempPath);
        }
    }
}
