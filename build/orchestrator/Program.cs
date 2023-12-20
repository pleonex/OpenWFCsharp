using BuildSystem;
using Cake.Core;
using Cake.Frosting;
using Cake.Frosting.PleOps.Recipe;
using Cake.Frosting.PleOps.Recipe.Dotnet;

return new CakeHost()
    .AddAssembly(typeof(PleOpsBuildContext).Assembly)
    .UseContext<BuildContext>()
    .UseLifetime<BuildLifetime>()
    .Run(args);

public sealed class BuildLifetime : FrostingLifetime<BuildContext>
{
    public override void Setup(BuildContext context, ISetupContext info)
    {
        // TODO: disable quality checks while prototyping.
        context.WarningsAsErrors = true;
        context.DotNetContext.CoverageTarget = 50;

        // Update build parameters from command line arguments.
        context.ReadArguments();

        context.DotNetContext.ApplicationProjects.Add(new ProjectPublicationInfo(
            "./src/OpenWFCsharp.Nas", [ "win-x64", "linux-x64", "osx-x64" ], "net8.0"));
        context.DotNetContext.ApplicationProjects.Add(new ProjectPublicationInfo(
            "./src/OpenWFCsharp.Dls", [ "win-x64", "linux-x64", "osx-x64" ], "net8.0"));

        context.OpenApiProjects.Add(new OpenApiProjectInfo(
            "nas",
            "src/OpenWFCsharp.Nas",
            $"bin/{context.DotNetContext.Configuration}/net8.0/OpenWFCsharp.Nas.dll"));
        context.OpenApiProjects.Add(new OpenApiProjectInfo(
            "dls",
            "src/OpenWFCsharp.Dls",
            $"bin/{context.DotNetContext.Configuration}/net8.0/OpenWFCsharp.Dls.dll"));

        context.DockerWebProject = "src/OpenWFCsharp.Backend";
        context.DockerImageName = "pleonex/openwfcsharp-complete";

        // Print the build info to use.
        context.Print();
    }

    public override void Teardown(BuildContext context, ITeardownContext info)
    {
        // Save the info from the existing artifacts for the next execution (e.g. deploy job)
        context.DeliveriesContext.Save();
    }
}

[TaskName("Default")]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Common.SetGitVersionTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Common.CleanArtifactsTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Dotnet.DotnetTasks.BuildProjectTask))]
public sealed class DefaultTask : FrostingTask
{
}

[TaskName("Bundle")]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Common.SetGitVersionTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.GitHub.ExportReleaseNotesTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Dotnet.DotnetTasks.BundleProjectTask))]
//TODO: [IsDependentOn(typeof(BuildDockerImageTask))]
[IsDependentOn(typeof(ExportOpenApiDocsTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.DocFx.BuildTask))]
public sealed class BundleTask : FrostingTask
{
}

[TaskName("Deploy")]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Common.SetGitVersionTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.Dotnet.DotnetTasks.DeployProjectTask))]
[IsDependentOn(typeof(Cake.Frosting.PleOps.Recipe.GitHub.UploadReleaseBinariesTask))]
public sealed class DeployTask : FrostingTask
{
}
