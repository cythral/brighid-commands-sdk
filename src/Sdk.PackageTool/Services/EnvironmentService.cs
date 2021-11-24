using System;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <inheritdoc />
    public class EnvironmentService : IEnvironmentService
    {
        /// <inheritdoc />
        public int ExitCode
        {
            get => Environment.ExitCode;
            set => Environment.ExitCode = value;
        }

        /// <inheritdoc />
        public string[] GetCommandLineArguments()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
