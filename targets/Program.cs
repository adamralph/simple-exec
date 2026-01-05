using static Bullseye.Targets;
using static Targets.Command;

Target("restore", () => RunAsync("dotnet", "restore"));

Target("format", dependsOn: ["restore",], () => RunAsync("dotnet", "format --verify-no-changes --no-restore"));

Target("build", dependsOn: ["restore",], () => RunAsync("dotnet", "build --configuration Release --no-restore"));

Target("test", dependsOn: ["build",], () => RunAsync("dotnet", "test --configuration Release --no-build"));

Target("pack", dependsOn: ["build",], () => RunAsync("dotnet", "pack --configuration Release --output artifacts --no-build"));

Target("default", dependsOn: ["format", "test", "pack",]);

await RunTargetsAndExitAsync(args, ex => ex.GetType() == ExitCodeExceptionType);
