using System.Runtime.CompilerServices;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class EchoingCommands
{
    [Fact]
    public static void EchoingACommand()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}");

        // assert
        Assert.Contains(TestName(), Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void EchoingACommandWithAnArgList()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", new[] { "exec", Tester.Path, "he llo", "\"world \"today\"", });

        // assert
        var lines = Capture.Out.ToString()!.Split('\r', '\n').ToList();
        Assert.Contains(lines, line => line.EndsWith(":   exec", StringComparison.Ordinal));
        Assert.Contains(lines, line => line.EndsWith($":   {Tester.Path}", StringComparison.Ordinal));
        Assert.Contains(lines, line => line.EndsWith(":   he llo", StringComparison.Ordinal));
        Assert.Contains(lines, line => line.EndsWith(":   \"world \"today\"", StringComparison.Ordinal));
    }

    [Fact]
    public static void SuppressingCommandEcho()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: true);

        // assert
        Assert.DoesNotContain(TestName(), Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void EchoingACommandWithASpecificPrefix()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: false, echoPrefix: $"{TestName()} prefix");

        // assert
        var error = Capture.Out.ToString()!;

        Assert.Contains(TestName(), error, StringComparison.Ordinal);
        Assert.Contains($"{TestName()} prefix:", error, StringComparison.Ordinal);
    }

    [Fact]
    public static void SuppressingCommandEchoWithASpecificPrefix()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: true, echoPrefix: $"{TestName()} prefix");

        // assert
        var error = Capture.Out.ToString()!;

        Assert.DoesNotContain(TestName(), error, StringComparison.Ordinal);
        Assert.DoesNotContain($"{TestName()} prefix:", error, StringComparison.Ordinal);
    }

    private static string TestName([CallerMemberName] string _ = "") => _;
}
