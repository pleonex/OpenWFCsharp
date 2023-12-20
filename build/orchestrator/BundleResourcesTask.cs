namespace BuildSystem;

using Cake.Common.IO;
using Cake.Core.Diagnostics;
using Cake.Frosting;

[TaskName("BundleResources")]
public class BundleResourcesTask : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        foreach (string resource in context.ResourcesToBundle) {
            context.Log.Information(resource);
            string name = Path.GetFileName(resource);
            string output = Path.Combine(context.ArtifactsPath, name);

            if (File.Exists(resource)) {
                File.Copy(resource, output, true);
                context.DeliveriesContext.BinaryFiles.Add(output);
            } else if (Directory.Exists(resource)) {
                output += ".zip";
                context.Zip(resource, output);
                context.DeliveriesContext.BinaryFiles.Add(output);
            } else {
                throw new FileNotFoundException("Missing resource", resource);
            }
        }
    }
}