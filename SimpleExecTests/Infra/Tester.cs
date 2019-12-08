namespace SimpleExecTests.Infra
{
    internal static class Tester
    {
        public static string Path =>
#if DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp3.1/SimpleExecTester.dll";
#else
            $"../../../../SimpleExecTester/bin/Release/netcoreapp3.1/SimpleExecTester.dll";
#endif
    }
}
