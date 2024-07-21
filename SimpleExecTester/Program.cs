using System.Text;

namespace SimpleExecTester;

internal static class Program
{
    public static int Main(string[] args)
    {
        if (args.Contains("unicode"))
        {
            Console.OutputEncoding = Encoding.Unicode;
            args = [.. args, "Pi (\u03a0)"];
        }

        Console.Out.WriteLine($"Arg count: {args.Length}");

        var input = args.Contains("in")
            ? Console.In.ReadToEnd()
                .Replace("\r", "\\r", StringComparison.Ordinal)
                .Replace("\n", "\\n", StringComparison.Ordinal)
            : null;

        Console.Out.WriteLine($"SimpleExecTester (stdin): {input}");
        Console.Out.WriteLine($"SimpleExecTester (stdout): {string.Join(" ", args)}");
        Console.Error.WriteLine($"SimpleExecTester (stderr): {string.Join(" ", args)}");

        if (args.Contains("large"))
        {
            Console.Out.WriteLine(new string('x', (int)Math.Pow(2, 12)));
            Console.Error.WriteLine(new string('x', (int)Math.Pow(2, 12)));
        }

        var exitCode = 0;
        if (args.FirstOrDefault(arg => int.TryParse(arg, out exitCode)) != null)
        {
            return exitCode;
        }

        if (args.Contains("sleep"))
        {
            Thread.Sleep(Timeout.Infinite);
            return 0;
        }

        Console.WriteLine($"foo={Environment.GetEnvironmentVariable("foo")}");

        return 0;
    }
}
