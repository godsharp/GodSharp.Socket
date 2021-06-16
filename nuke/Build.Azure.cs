using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] {"master","dev","release"},
    InvokedTargets = new[] {nameof(Push)},
    NonEntryTargets = new []{nameof(Initial),nameof(Clean),nameof(Restore),nameof(Compile),nameof(Copy),nameof(Artifacts)},
    ImportSecrets = new[] {nameof(NuGetApiKey), nameof(MyGetApiKey)},
    CacheKeyFiles = new string[0]
)]
partial class Build
{
}