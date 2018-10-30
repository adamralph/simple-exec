namespace SimpleExecTester
{
    using System;
    using System.Linq;
    using System.Threading;

    internal class Program
    {
        public static int Main(string[] args)
        {
            Console.Out.WriteLine($"SimpleExecTester (stdout): {string.Join(" ", args)}");

            if (args.Contains("error"))
            {
                Console.Error.WriteLine($"SimpleExecTester (stderr): {string.Join(" ", args)}");
                return 1;
            }
            else if (args.Length > 0 && int.TryParse(args[0], out var sleepInMilliseconds))
            {
                Thread.Sleep(sleepInMilliseconds);
                return 0;
            }

            return 0;
        }
    }
}
