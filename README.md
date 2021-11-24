# SimpleExec

![SimpleExec](https://raw.githubusercontent.com/adamralph/simple-exec/092a28b5dcd011725cef7f3b207fcb9a056b651d/assets/simple-exec.svg)

_[![NuGet version](https://img.shields.io/nuget/v/SimpleExec.svg?style=flat)](https://www.nuget.org/packages/SimpleExec)_

_[![Build status](https://github.com/adamralph/simple-exec/workflows/.github/workflows/ci.yml/badge.svg)](https://github.com/adamralph/simple-exec/actions/workflows/ci.yml?query=branch%3Amain)_
_[![CodeQL analysis](https://github.com/adamralph/simple-exec/workflows/.github/workflows/codeql-analysis.yml/badge.svg)](https://github.com/adamralph/simple-exec/actions/workflows/codeql-analysis.yml?query=branch%3Amain)_
_[![Lint](https://github.com/adamralph/simple-exec/workflows/.github/workflows/lint.yml/badge.svg)](https://github.com/adamralph/simple-exec/actions/workflows/lint.yml?query=branch%3Amain)_
_[![Spell check](https://github.com/adamralph/simple-exec/workflows/.github/workflows/spell-check.yml/badge.svg)](https://github.com/adamralph/simple-exec/actions/workflows/spell-check.yml?query=branch%3Amain)_

SimpleExec is a [.NET library](https://www.nuget.org/packages/SimpleExec) that runs external commands. It wraps [`System.Diagnostics.Process`](https://apisof.net/catalog/System.Diagnostics.Process) to make things easier.

SimpleExec intentionally does not invoke the system shell.

By default, the command is echoed to standard error (stderr) for visibility.

Platform support: [.NET Standard 2.1 and later](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

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
Run("foo.exe", "arg1 arg2", "my-working-directory");

await RunAsync("foo.exe");
await RunAsync("foo.exe", "arg1 arg2", "my-working-directory");
```

### Read

```C#
// ReadAsync returns a CommandReadResult object with Out and Error properties representing standard out (stdout) and standard error (stderr)
var result1 = await ReadAsync("foo.exe");
var result2 = await ReadAsync("foo.exe", "arg1 arg2", "my-working-directory");
```

### Other optional arguments

```C#
bool noEcho = false,
string? windowsName = null,
string? windowsArgs = null,
string? echoPrefix = null,
Action<IDictionary<string, string>>? configureEnvironment = null,
bool createNoWindow = false,
Func<int, bool>? handleExitCode = null,
CancellationToken cancellationToken = default,
```

### Exceptions

If the command has a non-zero exit code, an `ExitCodeException` is thrown with an `int` `ExitCode` property and a message in the form of:

```C#
$"The process exited with code {ExitCode}."
```

In the case of `ReadAsync`, an `ExitCodeReadException` is thrown, which inherits from `ExitCodeException`, and has `string` `Out` and `Error` properties, representing standard out (stdout) and standard error (stderr), and a message in the form of:

```C#
$@"The process exited with code {ExitCode}.

Standard Output:

{Out}

Standard Error:

{Error}"
```

#### Overriding default exit code handling

The throwing of exceptions for specific non-zero exit codes can be suppressed by passing a delegate to `handleExitCode` which returns `true` when it has handled the exit code and default exit code handling should be suppressed, and returns `false` otherwise. For example:

```C#
Run("ROBOCOPY", "from to", handleExitCode: code => code < 8);
```

Note that it may be useful to record the exit code. For example:

```C#
var exitCode = 0;
var result = Run("ROBOCOPY", "from to", handleExitCode: code => (exitCode = code) < 8);

// see https://ss64.com/nt/robocopy-exit.html
var oneOrMoreFilesCopied = exitCode & 1;
var extraFilesOrDirectoriesDetected = exitCode & 2;
var misMatchedFilesOrDirectoriesDetected = exitCode & 4;
```

### Windows

🙄

Sometimes, for whatever wonderful reasons, it's necessary to run a different command on Windows. For example, when running [Yarn](https://yarnpkg.com), some combination of mysterious factors may require that you explicitly run `cmd.exe` with Yarn as an argument, rather than running Yarn directly. The optional `windowsNames` and `windowsArgs` parameters may be used to specify a different command name and arguments for Windows:

```c#
Run("yarn", windowsName: "cmd", windowsArgs: "/c yarn");
```

---

<sub>[Run](https://thenounproject.com/term/target/975371) by [Gregor Cresnar](https://thenounproject.com/grega.cresnar/) from [the Noun Project](https://thenounproject.com/).</sub>
