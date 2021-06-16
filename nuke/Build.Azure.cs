using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AutoGenerate = true,
    TriggerBranchesInclude = new[] {"main"},
    InvokedTargets = new[] {nameof(Push)},
    ImportSecrets = new[] {nameof(NuGetApiKey), nameof(MyGetApiKey)}
)]
partial class Build
{
}