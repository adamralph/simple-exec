using System.Runtime.CompilerServices;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class EchoingCommands
{
    private const string SecretLower = "secret";
    private const string SecretUpper = "SECRET";

    private static Ct Ct => TestContext.Current.CancellationToken;

    [Fact]
    public static void EchoingACommand()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", ct: Ct);

        // assert
        Assert.Contains(TestName(), Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void EchoingACommandWithAnArgList()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", ["exec", Tester.Path, "he llo", "\"world \"today\"",], ct: Ct);

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
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: true, ct: Ct);

        // assert
        Assert.DoesNotContain(TestName(), Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void RedactingSecretsFromEchoPrefix()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path}", secrets: [SecretLower,], echoPrefix: $"{SecretLower}_{SecretUpper}", noEcho: false, ct: Ct);

        // assert
        Assert.DoesNotContain(SecretLower, Capture.Out.ToString()!, StringComparison.Ordinal);
        Assert.DoesNotContain(SecretUpper, Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void RedactingSecretsFromWorkingDirectory()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        _ = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path}", workingDirectory: $"{SecretLower}_{SecretUpper}", secrets: [SecretLower,], noEcho: false, ct: Ct));

        // assert
        Assert.DoesNotContain(SecretLower, Capture.Out.ToString()!, StringComparison.Ordinal);
        Assert.DoesNotContain(SecretUpper, Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void RedactingSecretsFromFileName()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        _ = Record.Exception(() => Command.Run($"{SecretLower}_{SecretUpper}", secrets: [SecretLower,], noEcho: false, ct: Ct));

        // assert
        Assert.DoesNotContain(SecretLower, Capture.Out.ToString()!, StringComparison.Ordinal);
        Assert.DoesNotContain(SecretUpper, Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void RedactingSecretsFromArguments()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {SecretLower} {SecretUpper}", secrets: [SecretLower,], noEcho: false, ct: Ct);

        // assert
        Assert.DoesNotContain(SecretLower, Capture.Out.ToString()!, StringComparison.Ordinal);
        Assert.DoesNotContain(SecretUpper, Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void RedactingSecretsFromArgumentsList()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", ["exec", Tester.Path, SecretLower, SecretUpper,], secrets: [SecretLower,], noEcho: false, ct: Ct);

        // assert
        Assert.DoesNotContain(SecretLower, Capture.Out.ToString()!, StringComparison.Ordinal);
        Assert.DoesNotContain(SecretUpper, Capture.Out.ToString()!, StringComparison.Ordinal);
    }

    [Fact]
    public static void EchoingACommandWithASpecificPrefix()
    {
        // arrange
        Console.SetOut(Capture.Out);

        // act
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", echoPrefix: $"{TestName()} prefix", noEcho: false, ct: Ct);

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
        Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", echoPrefix: $"{TestName()} prefix", noEcho: true, ct: Ct);

        // assert
        var error = Capture.Out.ToString()!;

        Assert.DoesNotContain(TestName(), error, StringComparison.Ordinal);
        Assert.DoesNotContain($"{TestName()} prefix:", error, StringComparison.Ordinal);
    }

    private static string TestName([CallerMemberName] string _ = "") => _;
}
