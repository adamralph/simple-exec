using System;

namespace SimpleExec
{
    [Obsolete("Use ExitCodeException instead.", true)]
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class NonZeroExitCodeException : Exception
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CA1032 // Implement standard exception constructors
    {
    }
}
