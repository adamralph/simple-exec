<img src="assets/simple-exec.png" width="100" />

# SimpleExec

_[![NuGet version](https://img.shields.io/nuget/v/SimpleExec.svg?style=flat)](https://www.nuget.org/packages/SimpleExec)_
_[![Build status](https://github.com/adamralph/simple-exec/workflows/.github/workflows/ci.yml/badge.svg)](https://github.com/adamralph/simple-exec/actions)_

SimpleExec is a [.NET library](https://www.nuget.org/packages/SimpleExec) that runs external commands. It wraps [`System.Diagnostics.Process`](https://apisof.net/catalog/System.Diagnostics.Process) to make things easier.

SimpleExec intentionally does not invoke the system shell.

The command is echoed to standard error (stderr) for visibility.

Platform support: [.NET Standard 1.3 and upwards](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

## Quick start

```C#
using static SimpleExec.Command;
```

```C#
Run("foo.exe", "arg1 arg2");
```

## API

### Run

```C#
Run("foo.exe");
Run("foo.exe", "arg1 arg2", "my-directory", noEcho: true);

await RunAsync("foo.exe");
await RunAsync("foo.exe", "arg1 arg2", "my-directory", noEcho: true);
```

### Read

```C#
var output1 = Read("foo.exe");
var output2 = Read("foo.exe", "arg1 arg2", "my-directory", noEcho: true);

var output3 = await ReadAsync("foo.exe");
var output4 = await ReadAsync("foo.exe", "arg1 arg2", "my-directory", noEcho: true);
```

### Exceptions

If the command has a non-zero exit code, a `NonZeroExitCodeException` is thrown with an `int` `ExitCode` property and a message in the form of:

```C#
$"The process exited with code {ExitCode}."
```

---

<sub>[Run](https://thenounproject.com/term/target/975371) by [Gregor Cresnar](https://thenounproject.com/grega.cresnar/) from [the Noun Project](https://thenounproject.com/).</sub>
