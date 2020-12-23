using System;
using System.Linq;
using System.Threading;

namespace SimpleExecTester
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            Console.Out.WriteLine($"SimpleExecTester (stdout): {string.Join(" ", args)}");

            if (args.Contains("large"))
            {
                Console.WriteLine(new string('x', (int)Math.Pow(2, 12)));
            }

            if (args.Contains("error"))
            {
                Console.Error.WriteLine($"SimpleExecTester (stderr): {string.Join(" ", args)}");
                return 1;
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
}
