namespace BuildSystem;

using System.Text;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;
using Cake.Frosting;
using Cake.Frosting.PleOps.Recipe.Common;

[TaskName("ExportOpenApiDocs")]
[IsDependentOn(typeof(RestoreToolsTask))]
public class ExportOpenApiDocsTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (OpenApiProjectInfo info in context.OpenApiProjects) {
            ExportProject(context, info);
        }
    }

    private void ExportProject(BuildContext context, OpenApiProjectInfo info)
    {
        context.Log.Information(
            "Export OpenAPI document: {0} from {1}/{2}",
            info.DocumentName,
            info.DotnetProjectPath,
            info.AssemblyPath);

        string outputFilename = $"{info.DocumentName}.swagger.json";
        string outputFile = Path.Combine(context.ArtifactsPath, outputFilename);

        string args = new StringBuilder()
            .Append("tofile ")
            .AppendFormat("--output \"{0}\" ", outputFile)
            .Append("--serializeasv2 ") // compatibility with DocFX
            .AppendFormat("\"{0}\" ", info.AssemblyPath)
            .Append(info.DocumentName)
            .ToString();

        context.DotNetTool(
            "swagger " + args,
            new DotNetToolSettings()
                .WithEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development")
                .WithWorkingDirectory(info.DotnetProjectPath));

        // Copy to DocFx docs too
        if (!string.IsNullOrEmpty(context.OpenApiDocFxPath)) {
            if (!Directory.Exists(context.OpenApiDocFxPath)) {
                Directory.CreateDirectory(context.OpenApiDocFxPath);
            }

            string docsFile = Path.Combine(context.OpenApiDocFxPath, outputFilename);
            File.Copy(outputFile, docsFile, true);
        }

        // Mark to upload as an artifact in the GitHub release
        context.DeliveriesContext.BinaryFiles.Add(outputFile);
    }
}