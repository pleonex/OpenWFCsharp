namespace BuildSystem;

using System.Text;
using Cake.Common;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("DeployDockerImages")]
public class DeployDockerImagesTask : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return context.BuildKind is Cake.Frosting.PleOps.Recipe.BuildKind.Stable;
    }

    public override void Run(BuildContext context)
    {
        foreach (DotnetDockerProjectInfo info in context.DockerProjects) {
            PushImage(context, info);
        }
    }

    private void PushImage(BuildContext context, DotnetDockerProjectInfo projectInfo)
    {
        context.Log.Information("Pushing Docker image: {0}", projectInfo.ImageName);

        // No prefix for DockerHub
        string registryPrefix = string.IsNullOrEmpty(context.DockerRegistry) ? "" : $"{context.DockerRegistry}/";

        string dockerArgs = new StringBuilder()
            .Append("push")
            .Append($" {registryPrefix}{projectInfo.ImageName}:{context.Version}")
            .ToString();
        int retCode = context.StartProcess("docker", dockerArgs);
        if (retCode != 0) {
            throw new Exception("Docker deploy failed!");
        }
    }
}
