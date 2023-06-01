using System.Security.Claims;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Context holder for executing commands.
    /// </summary>
    /// <typeparam name="TInput">The type of input to use.</typeparam>
    public class CommandContext<TInput>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext{TInput}" /> class.
        /// </summary>
        /// <param name="input">The input parameter to use when invoking the command.</param>
        /// <param name="principal">The principal invoking the command.</param>
        /// <param name="sourceSystem">The name of the system the command is being invoked from.</param>
        /// <param name="sourceSystemId">ID of the sender/channel in the source system the command request originated from.</param>
        /// <param name="token">Token of the user executing the command.</param>
        public CommandContext(
            TInput input,
            ClaimsPrincipal principal,
            string sourceSystem,
            string sourceSystemId,
            string? token
        )
        {
            Input = input;
            Principal = principal;
            SourceSystem = sourceSystem;
            SourceSystemId = sourceSystemId;
            Token = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext{TInput}" /> class.
        /// </summary>
        /// <param name="input">The input parameter to use when invoking the command.</param>
        /// <param name="principal">The principal invoking the command.</param>
        /// <param name="sourceSystem">The name of the system this command is being invoked from.</param>
        /// <param name="sourceSystemChannel">ID of the channel in the source system the command request originated from.</param>
        /// <param name="sourceSystemUser">ID of the user in the source system the command request originated from.</param>
        /// <param name="token">Token of the user executing the command.</param>
        public CommandContext(
            TInput input,
            ClaimsPrincipal principal,
            string sourceSystem,
            string sourceSystemChannel,
            string sourceSystemUser,
            string? token
        )
        {
            Input = input;
            Principal = principal;
            SourceSystem = sourceSystem;
            SourceSystemChannel = sourceSystemChannel;
            SourceSystemUser = sourceSystemUser;
            Token = token;
        }

        /// <summary>
        /// Gets the input stream containing command arguments and options.
        /// </summary>
        public TInput Input { get; }

        /// <summary>
        /// Gets the principal invoking the command.
        /// </summary>
        public ClaimsPrincipal Principal { get; }

        /// <summary>
        /// Gets the source platform the command is being invoked from.
        /// </summary>
        public string SourceSystem { get; }

        /// <summary>
        /// Gets the ID of the sender/channel within the source system the command request originated from.
        /// </summary>
        public string SourceSystemId { get; } = string.Empty;

        /// <summary>
        /// Gets the ID of the channel within the source system the command request originated from.
        /// </summary>
        public string SourceSystemChannel { get; } = string.Empty;

        /// <summary>
        /// Gets the ID of the user within the source system the command request originated from.
        /// </summary>
        public string SourceSystemUser { get; } = string.Empty;

        /// <summary>
        /// Gets the token of the user who is executing the command.  In order for the token to be passed to the command context,
        /// the command must have the 'token' scope.
        /// </summary>
        public string? Token { get; }
    }
}
