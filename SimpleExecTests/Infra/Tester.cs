namespace SimpleExecTests.Infra
{
    internal static class Tester
    {
        public static string Path =>
#if DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp2.1/SimpleExecTester.dll";
#else
            $"../../../../SimpleExecTester/bin/Release/netcoreapp2.1/SimpleExecTester.dll";
#endif
    }
}
