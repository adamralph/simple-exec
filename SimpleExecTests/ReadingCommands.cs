using System.ComponentModel;
using System.Text;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class ReadingCommands
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static async Task ReadingACommandAsync(bool largeOutput)
    {
        // act
        var (standardOutput, standardError) = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : ""));

        // assert
        Assert.Contains("hello world", standardOutput, StringComparison.Ordinal);
        Assert.Contains("hello world", standardError, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static async Task ReadingACommandAsyncWithAnArgList(bool largeOutput)
    {
        // arrange
        var args = new List<string> { "exec", Tester.Path, "he llo", "world", };
        if (largeOutput)
        {
            args.Add("large");
        }

        // act
        var (standardOutput, standardError) = await Command.ReadAsync("dotnet", args);

        // assert
        Assert.Contains(largeOutput ? "Arg count: 3" : "Arg count: 2", standardOutput, StringComparison.Ordinal);
        Assert.Contains("he llo world", standardOutput, StringComparison.Ordinal);
        Assert.Contains("he llo world", standardError, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static async Task ReadingACommandWithInputAsync(bool largeOutput)
    {
        // act
        var (standardOutput, standardError) = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world in" + (largeOutput ? " large" : ""), standardInput: "this is input");

        // assert
        Assert.Contains("hello world", standardOutput, StringComparison.Ordinal);
        Assert.Contains("this is input", standardOutput, StringComparison.Ordinal);
        Assert.Contains("hello world", standardError, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static async Task ReadingAUnicodeCommandAsync(bool largeOutput)
    {
        // act
        var (standardOutput, standardError) = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding());

        // assert
        Assert.Contains("Pi (\u03a0)", standardOutput, StringComparison.Ordinal);
        Assert.Contains("Pi (\u03a0)", standardError, StringComparison.Ordinal);
    }

    [Fact]
    public static async Task ReadingAFailingCommandAsync()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path} 1 hello world"));

        // assert
        var exitCodeReadException = Assert.IsType<ExitCodeReadException>(exception);
        Assert.Equal(1, exitCodeReadException.ExitCode);
        Assert.Contains("hello world", exitCodeReadException.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("hello world", exitCodeReadException.StandardError, StringComparison.Ordinal);
    }

    [Fact]
    public static async Task ReadingACommandAsyncInANonExistentWorkDirectory()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path}", "non-existent-working-directory"));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Fact]
    public static async Task ReadingANonExistentCommandAsync()
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync("simple-exec-tests-non-existent-command"));

        // assert
        _ = Assert.IsType<Win32Exception>(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public static async Task ReadingNoCommandAsync(string name)
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync(name));

        // assert
        Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
    }
}
