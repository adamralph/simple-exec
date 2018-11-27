<img src="assets/simple-exec.png" width="100" />

# SimpleExec

_[![nuget](https://img.shields.io/nuget/v/SimpleExec.svg?style=flat)](https://www.nuget.org/packages/SimpleExec)_
_[![build](https://ci.appveyor.com/api/projects/status/sagnyx3o2x0bidm1/branch/master?svg=true)](https://ci.appveyor.com/project/adamralph/simple-exec/branch/master)_

SimpleExec is a [.NET package](https://www.nuget.org/packages/SimpleExec) that runs external commands. It wraps [`System.Diagnostics.Process`](https://apisof.net/catalog/System.Diagnostics.Process) to make things easier.

The SimpleExec package intentionally does not invoke the system shell.

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

Each method has:

- One mandatory parameter: `string name`.
- Three optional parameters: `string args`, `string workingDirectory`, and `bool noEcho`.

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

var output3 = ReadAsync("foo.exe");
var output4 = ReadAsync("foo.exe", "arg1 arg2", "my-directory", noEcho: true);
```

### Exceptions

If the command has a non-zero exit code, an exception is thrown with a message in the form of:

```C#
$"The process exited with code {process.ExitCode}."
```

---

<sub>[Run](https://thenounproject.com/term/target/975371) by [Gregor Cresnar](https://thenounproject.com/grega.cresnar/) from [the Noun Project](https://thenounproject.com/).</sub>
