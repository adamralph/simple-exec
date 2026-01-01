using static Bullseye.Targets;
using static Targets.Command;

Target("format", () => RunAsync("dotnet", "format --verify-no-changes"));

Target("build", () => RunAsync("dotnet", "build --configuration Release --nologo"));

Target("test", dependsOn: ["build",], () => RunAsync("dotnet", "test --configuration Release --no-build"));

Target("pack", dependsOn: ["build",], () => RunAsync("dotnet", "pack --configuration Release --output artifacts --no-build"));

Target("default", dependsOn: ["format", "test", "pack",]);

await RunTargetsAndExitAsync(args, ex => ex.GetType() == ExitCodeExceptionType);
