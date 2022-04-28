using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    "dev",
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] { "dev" },
    InvokedTargets = new[] { nameof(Compile) },
    NonEntryTargets = new[] { nameof(Initial), nameof(Clean), nameof(Restore) },
    CacheKeyFiles = new string[0]
)]
[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] { "master" },
    InvokedTargets = new[] { nameof(Push) },
    NonEntryTargets = new[] { nameof(Initial), nameof(Clean), nameof(Restore), nameof(Compile), nameof(Copy), nameof(Artifacts) },
    ImportSecrets = new[] { nameof(NuGetApiKey), nameof(MyGetApiKey) },
    CacheKeyFiles = new string[0]
)]
partial class Build
{
}