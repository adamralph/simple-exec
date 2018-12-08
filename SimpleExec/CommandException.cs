namespace SimpleExec
{
    using System;

    [Obsolete("Use NonZeroExitCodeException instead. This type will be removed in 5.0.0.")]
    public class CommandException : Exception
    {
        public CommandException()
        {
        }

        public CommandException(string message) : base(message)
        {
        }

        public CommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
