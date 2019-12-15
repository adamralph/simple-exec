namespace SimpleExecTester
{
    using System;
    using System.Linq;

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

            Console.WriteLine($"foo={Environment.GetEnvironmentVariable("foo")}");

            return 0;
        }
    }
}
