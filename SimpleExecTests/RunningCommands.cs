using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class RunningCommands
{
    private static readonly string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "hello-world.cmd" : "ls";
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    public static bool OsPlatformIsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [Fact]
    public static void RunningASucceedingCommand()
    {
        // act
        var exception = Record.Exception(() => Command.Run(command, cancellationToken: Ct));

        // assert
        Assert.Null(exception);
    }

    [Fact]
    public static void RunningASucceedingCommandWithArgs()
    {
        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} hello world", cancellationToken: Ct));

        // assert
        Assert.Null(exception);
    }

    [Fact]
    public static async Task RunningASucceedingCommandAsync()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync(command, cancellationToken: Ct));

        // assert
        Assert.Null(exception);
    }

    [Fact]
    public static void RunningAFailingCommand()
    {
        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} 1 hello world", cancellationToken: Ct));

        // assert
        Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
    }

    [Fact]
    public static async Task RunningAFailingCommandAsync()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path} 1 hello world", cancellationToken: Ct));

        // assert
        Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
    }

    [Fact]
    public static void RunningACommandInANonExistentWorkDirectory()
    {
        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path}", "non-existent-working-directory", cancellationToken: Ct));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Fact]
    public static async Task RunningACommandAsyncInANonExistentWorkDirectory()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path}", "non-existent-working-directory", cancellationToken: Ct));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Fact]
    public static void RunningANonExistentCommand()
    {
        // act
        var exception = Record.Exception(() => Command.Run("simple-exec-tests-non-existent-command", cancellationToken: Ct));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Fact]
    public static async Task RunningANonExistentCommandAsync()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("simple-exec-tests-non-existent-command", cancellationToken: Ct));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public static void RunningNoCommand(string name)
    {
        // act
        var exception = Record.Exception(() => Command.Run(name, cancellationToken: Ct));

        // assert
        Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public static async Task RunningNoCommandAsync(string name)
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync(name, cancellationToken: Ct));

        // assert
        Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
    }

    [Fact(SkipUnless = nameof(OsPlatformIsWindows), Skip = "OS platform is not Windows")]
    public static async Task RunningCommandsInPathOnWindows()
    {
        // arrange
        var directory = Path.Combine(
            Path.GetTempPath(),
            "SimpleExecTests",
            DateTimeOffset.UtcNow.UtcTicks.ToString(CultureInfo.InvariantCulture),
            "RunningCommandsInPathOnWindows");

        _ = Directory.CreateDirectory(directory);

        if (!SpinWait.SpinUntil(() => Directory.Exists(directory), 50))
        {
            throw new IOException($"Failed to create directory '{directory}'.");
        }

        var name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        var fullName = Path.Combine(directory, Path.ChangeExtension(name, "cmd"));
        await File.WriteAllTextAsync(fullName, "echo foo", cancellationToken: Ct);

        Environment.SetEnvironmentVariable(
            "PATH",
            $"{Environment.GetEnvironmentVariable("PATH")}{Path.PathSeparator}{directory}",
            EnvironmentVariableTarget.Process);

        // act
        var exception = Record.Exception(() => Command.Run(name, cancellationToken: Ct));

        // assert
        Assert.Null(exception);
    }

    [Theory(SkipUnless = nameof(OsPlatformIsWindows), Skip = "OS platform is not Windows")]
    [InlineData(".BAT;.CMD", "hello from bat")]
    [InlineData(".CMD;.BAT", "hello from cmd")]
    public static async Task RunningCommandsInPathOnWindowsWithSpecificPathExt(
        string pathExt, string expected)
    {
        // arrange
        var directory = Path.Combine(
            Path.GetTempPath(),
            "SimpleExecTests",
            DateTimeOffset.UtcNow.UtcTicks.ToString(CultureInfo.InvariantCulture),
            "RunningCommandsInPathOnWindows");

        _ = Directory.CreateDirectory(directory);

        if (!SpinWait.SpinUntil(() => Directory.Exists(directory), 50))
        {
            throw new IOException($"Failed to create directory '{directory}'.");
        }

        var name = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        var batName = Path.Combine(directory, Path.ChangeExtension(name, "bat"));
        await File.WriteAllTextAsync(batName, "@echo hello from bat", cancellationToken: Ct);

        var cmdName = Path.Combine(directory, Path.ChangeExtension(name, "cmd"));
        await File.WriteAllTextAsync(cmdName, "@echo hello from cmd", cancellationToken: Ct);

        Environment.SetEnvironmentVariable(
            "PATH",
            $"{Environment.GetEnvironmentVariable("PATH")}{Path.PathSeparator}{directory}",
            EnvironmentVariableTarget.Process);

        Environment.SetEnvironmentVariable(
            "PATHEXT",
            pathExt,
            EnvironmentVariableTarget.Process);

        // act
        var actual = (await Command.ReadAsync(name, cancellationToken: Ct)).StandardOutput.Trim();

        // assert
        Assert.Equal(expected, actual);
    }
}
