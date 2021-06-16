using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.AzurePipelines;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Logger = Nuke.Common.Logger;

// ReSharper disable InconsistentNaming

[CheckBuildProjectConfigurations]
partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Push);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("MyGet Api Key")]
    [Secret]
    string MyGetApiKey;
    
    [Parameter("NuGet Api Key")]
    [Secret]
    string NuGetApiKey;

    [Solution] readonly Solution Solution;

    [CI] readonly AzurePipelines AzurePipelines;
    [CI] readonly GitHubActions GitHubActions;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Initial => _ => _
        .Description("Initial")
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            MyGetApiKey ??= Environment.GetEnvironmentVariable("MyGetApiKey");
            NuGetApiKey ??= Environment.GetEnvironmentVariable("NuGetApiKey");
        });
    
    Target Clean => _ => _
        .Description("Clean Solution")
        .DependsOn(Initial)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .Description("Restore Solution")
        .DependsOn(Clean)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Restore"));
        });

    Target Compile => _ => _
        .Description("Compile Solution")
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Rebuild")
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetNodeReuse(IsLocalBuild));
        });
    
    Target Copy => _ => _
        .Description("Copy NuGet Package")
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        .DependsOn(Compile)
        .Executes(() =>
        {
            GlobFiles(OutputDirectory, "**/*.nupkg")
                ?.Where(x => !x.EndsWith(".symbols.nupkg"))
                .ForEach(x => CopyFileToDirectory(x, ArtifactsDirectory / "packages", FileExistsPolicy.OverwriteIfNewer));
        });
    
    Target Artifacts => _ => _
        .DependsOn(Copy)
        .OnlyWhenStatic(() => IsServerBuild)
        .Description("Upload Artifacts")
        .Executes(() =>
        {
            Logger.Info("Upload artifacts to azure...");
            AzurePipelines
                .UploadArtifacts("artifacts", "artifacts", ArtifactsDirectory);
            Logger.Info("Upload artifacts to azure finished.");
        });
    
    Target Push => _ => _
        .Description("Push NuGet Package")
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        .DependsOn(Artifacts)
        .Requires(() => NuGetApiKey)
        .Requires(() => MyGetApiKey)
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory / "packages", "**/*.nupkg")
                ?.Where(x => !x.EndsWith(".symbols.nupkg"))
                .ForEach(Nuget);
        });

    void Nuget(string x)
    {
        Nuget(x, "https://www.myget.org/F/godsharplab/api/v2/package", MyGetApiKey);
        Nuget(x, "https://api.nuget.org/v3/index.json", NuGetApiKey);
    }

    void Nuget(string x, string source, string key) =>
        DotNetNuGetPush(s => s
            .SetTargetPath(x)
            .SetSource(source)
            .SetApiKey(key));
}