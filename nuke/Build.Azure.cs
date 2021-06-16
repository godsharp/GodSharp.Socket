using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] {"master","dev","release"},
    InvokedTargets = new[] {nameof(Push)},
    ImportSecrets = new[] {nameof(NuGetApiKey), nameof(MyGetApiKey)},
    CacheKeyFiles = null
)]
partial class Build
{
}