namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Service for interacting with the environment.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// Gets or sets the exit code to exit with when the program finishes.
        /// </summary>
        int ExitCode { get; set; }

        /// <summary>
        /// Gets the command line arguments from the environment.
        /// </summary>
        /// <returns>The command line arguments.</returns>
        string[] GetCommandLineArguments();
    }
}
