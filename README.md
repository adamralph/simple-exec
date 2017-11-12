# SimpleExec

SimpleExec is a [.NET package](https://www.nuget.org/packages/SimpleExec) that runs external commands. It wraps [`System.Diagnostics.Process`](https://apisof.net/catalog/System.Diagnostics.Process) to make things easier.

The SimpleExec package intentionally does not invoke the system shell.

The command is echoed in the console (stdout) for visibility.

Platform support: [.NET Standard 1.3 and upwards](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

## Quick start

```PowerShell
Install-Package SimpleExec
```

```C#
using static SimpleExec.Command;
```

```C#
Run("foo.exe", "arg1 arg2");
Run("foo.exe", "arg1 arg2", "working-directory");

await RunAsync("bar.exe", "arg1 arg2");
await RunAsync("foo.exe", "arg1 arg2", "working-directory");
```

If the command has a non-zero exit code, an exception is thrown with a message in the form of:

```C#
$"The process exited with code {process.ExitCode}: {stdErr.Trim()}"
```
