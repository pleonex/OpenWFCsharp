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

        context.ResourcesToBundle.Add("./storage");
        context.ResourcesToBundle.Add("./deployment");

        context.OpenApiProjects.Add(new OpenApiProjectInfo(
            "nas",
            "src/OpenWFCsharp.Nas",
            $"bin/{context.DotNetContext.Configuration}/net8.0/OpenWFCsharp.Nas.dll"));
        context.DockerProjects.Add(new DotnetDockerProjectInfo(
            "src/OpenWFCsharp.Nas", "pleonex/openwfcsharp-nas"));

        context.OpenApiProjects.Add(new OpenApiProjectInfo(
            "dls",
            "src/OpenWFCsharp.Dls",
            $"bin/{context.DotNetContext.Configuration}/net8.0/OpenWFCsharp.Dls.dll"));
        context.DockerProjects.Add(new DotnetDockerProjectInfo(
            "src/OpenWFCsharp.Dls", "pleonex/openwfcsharp-dls"));

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
[IsDependentOn(typeof(BundleResourcesTask))]
[IsDependentOn(typeof(BuildDotnetDockerImageTask))]
[IsDependentOn(typeof(ExportDockerImageTask))]
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
