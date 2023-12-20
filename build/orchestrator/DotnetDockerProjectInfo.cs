namespace BuildSystem;

public record DotnetDockerProjectInfo(
    string ProjectPath,
    string ImageName,
    string Architecture = "x64");
