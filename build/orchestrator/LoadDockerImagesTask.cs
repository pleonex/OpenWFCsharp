namespace BuildSystem;

using System.Text;
using Cake.Common;
using Cake.Common.IO;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("LoadDockerImages")]
public class LoadDockerImagesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (string image in context.DockerImageArtifacts) {
            ImportImage(context, image);
        }
    }

    private void ImportImage(BuildContext context, string zipPath)
    {
        context.Log.Information("Importing docker image: {0}", zipPath);

        // Unzip
        string tempPath = Path.Combine(context.TemporaryPath, Path.GetRandomFileName());
        context.Unzip(zipPath, tempPath);
        string imagePath = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(zipPath));

        try {
            string dockerArgs = new StringBuilder()
                .Append("load")
                .Append(" -i ").Append(imagePath)
                .ToString();
            int retCode = context.StartProcess("docker", dockerArgs);
            if (retCode != 0) {
                throw new Exception("Docker load failed!");
            }
        } finally {
            Directory.Delete(tempPath, true);
        }
    }
}
