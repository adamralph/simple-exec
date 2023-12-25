using System.Runtime.InteropServices;
using Xunit;

namespace SimpleExecTests.Infra;

internal sealed class WindowsTheoryAttribute : TheoryAttribute
{
    public override string Skip
    {
        get => !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows only" : base.Skip;
        set => base.Skip = value;
    }
}
