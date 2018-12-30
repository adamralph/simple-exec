namespace SimpleExecTests.Infra
{
    internal static class Tester
    {
        public static string Path =>
#if DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp2.2/SimpleExecTester.dll";
#else
            $"../../../../SimpleExecTester/bin/Release/netcoreapp2.2/SimpleExecTester.dll";
#endif
    }
}
