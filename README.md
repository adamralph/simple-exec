# SimpleExec

![SimpleExec](https://raw.githubusercontent.com/adamralph/simple-exec/092a28b5dcd011725cef7f3b207fcb9a056b651d/assets/simple-exec.svg)

_[![NuGet version](https://img.shields.io/nuget/v/SimpleExec.svg?style=flat)](https://www.nuget.org/packages/SimpleExec)_

_[![Build status](https://github.com/adamralph/simple-exec/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/adamralph/simple-exec/actions/workflows/ci.yml)_
_[![CodeQL analysis](https://github.com/adamralph/simple-exec/actions/workflows/codeql-analysis.yml/badge.svg?branch=main)](https://github.com/adamralph/simple-exec/actions/workflows/codeql-analysis.yml)_
_[![lint](https://github.com/adamralph/simple-exec/actions/workflows/lint.yml/badge.svg?branch=main)](https://github.com/adamralph/simple-exec/actions/workflows/lint.yml)_
_[![Spell check](https://github.com/adamralph/simple-exec/actions/workflows/spell-check.yml/badge.svg?branch=main)](https://github.com/adamralph/simple-exec/actions/workflows/spell-check.yml)_

SimpleExec is a [.NET library](https://www.nuget.org/packages/SimpleExec) that runs external commands. It wraps [`System.Diagnostics.Process`](https://apisof.net/catalog/System.Diagnostics.Process) to make things easier.

SimpleExec intentionally does not invoke the system shell.

Platform support: [.NET 6.0 and later](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

- [Quick start](#quick-start)
- [Run](#run)
- [Read](#read)
- [Other optional arguments](#other-optional-arguments)
- [Exceptions](#exceptions)

## Quick start

```c#
using static SimpleExec.Command;
```

```c#
Run("foo", "arg1 arg2");
```

## Run

```c#
Run("foo");
Run("foo", "arg1 arg2");
Run("foo", new[] { "arg1", "arg2" });

await RunAsync("foo");
await RunAsync("foo", "arg1 arg2");
await RunAsync("foo", new[] { "arg1", "arg2" });
```

By default, the command is echoed to standard output (stdout) for visibility.

## Read

```c#
var (standardOutput1, standardError1) = await ReadAsync("foo");
var (standardOutput2, standardError2) = await ReadAsync("foo", "arg1 arg2");
var (standardOutput3, standardError3) = await ReadAsync("foo", new[] { "arg1", "arg2" });
```

## Other optional arguments

```c#
string workingDirectory = "",
bool noEcho = false,
string? echoPrefix = null,
Action<IDictionary<string, string?>>? configureEnvironment = null,
bool createNoWindow = false,
Encoding? encoding = null,
Func<int, bool>? handleExitCode = null,
string? standardInput = null,
bool cancellationIgnoresProcessTree = false,
CancellationToken cancellationToken = default,
```

## Exceptions

If the command has a non-zero exit code, an `ExitCodeException` is thrown with an `int` `ExitCode` property and a message in the form of:

```c#
$"The process exited with code {ExitCode}."
```

In the case of `ReadAsync`, an `ExitCodeReadException` is thrown, which inherits from `ExitCodeException`, and has `string` `Out` and `Error` properties, representing standard out (stdout) and standard error (stderr), and a message in the form of:

```c#
$@"The process exited with code {ExitCode}.

Standard Output:

{Out}

Standard Error:

{Error}"
```

### Overriding default exit code handling

Most programs return a zero exit code when they succeed and a non-zero exit code fail. However, there are some programs which return a non-zero exit code when they succeed. For example, [Robocopy](https://ss64.com/nt/robocopy.html) returns an exit code less than 8 when it succeeds and 8 or greater when a failure occurs.

The throwing of exceptions for specific non-zero exit codes may be suppressed by passing a delegate to `handleExitCode` which returns `true` when it has handled the exit code and default exit code handling should be suppressed, and returns `false` otherwise.

For example, when running Robocopy, exception throwing should be suppressed for an exit code less than 8:

```c#
Run("ROBOCOPY", "from to", handleExitCode: code => code < 8);
```

Note that it may be useful to record the exit code. For example:

```c#
var exitCode = 0;
Run("ROBOCOPY", "from to", handleExitCode: code => (exitCode = code) < 8);

// see https://ss64.com/nt/robocopy-exit.html
var oneOrMoreFilesCopied = exitCode & 1;
var extraFilesOrDirectoriesDetected = exitCode & 2;
var misMatchedFilesOrDirectoriesDetected = exitCode & 4;
```

---

<sub>[Run](https://thenounproject.com/term/target/975371) by [Gregor Cresnar](https://thenounproject.com/grega.cresnar/) from [the Noun Project](https://thenounproject.com/).</sub>
