namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Represents the result of executing a command.
    /// </summary>
    public readonly struct CommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult" /> struct.
        /// </summary>
        /// <param name="message">The message returned by the command.</param>
        public CommandResult(
            string? message = null
        )
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message returned by a command execution.
        /// </summary>
        public string? Message { get; }
    }
}
