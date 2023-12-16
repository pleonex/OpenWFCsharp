namespace BuildSystem;

using System.Text;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Core.Tooling;
using Cake.Frosting;
using Cake.Frosting.PleOps.Recipe.Common;

[TaskName("ExportSwaggerFile")]
[IsDependentOn(typeof(RestoreToolsTask))]
public class ExportSwaggerFileTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        string outputFilename = $"{context.SwaggerDocName}.swagger.json";
        string outputFile = Path.Combine(context.ArtifactsPath, outputFilename);

        string args = new StringBuilder()
            .Append("tofile ")
            .AppendFormat("--output \"{0}\" ", outputFile)
            .Append("--serializeasv2 ") // compatibility with DocFX
            .AppendFormat("\"{0}\" ", context.SwaggerAssemblyPath)
            .Append(context.SwaggerDocName)
            .ToString();

        context.DotNetTool(
            "swagger " + args,
            new DotNetToolSettings()
                .WithEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development")
                .WithWorkingDirectory(context.SwaggerProjectPath));

        // Copy to DocFx docs too
        if (!Directory.Exists(context.SwaggerDocFxPath)) {
            Directory.CreateDirectory(context.SwaggerDocFxPath);
        }

        string docsFile = Path.Combine(context.SwaggerDocFxPath, outputFilename);
        File.Copy(outputFile, docsFile, true);

        // Mark to upload as an artifact in the GitHub release
        context.DeliveriesContext.BinaryFiles.Add(outputFile);
    }
}