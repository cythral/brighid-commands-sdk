using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Context holder for executing commands.
    /// </summary>
    public class CommandContext : IAsyncDisposable
    {
        private Stream? inputStream;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext" /> class.
        /// </summary>
        /// <param name="inputStream">The input stream containing command arguments and options.</param>
        /// <param name="principal">The principal invoking the command.</param>
        /// <param name="sourceSystem">The name of the system this command is being invoked from.</param>
        /// <param name="sourceSystemId">ID of the sender/channel in the source system the command request originated from.</param>
        /// <param name="token">Token of the user executing the command.</param>
        public CommandContext(
            Stream inputStream,
            ClaimsPrincipal principal,
            string sourceSystem,
            string sourceSystemId,
            string? token
        )
        {
            this.inputStream = inputStream;
            Principal = principal;
            SourceSystem = sourceSystem;
            SourceSystemId = sourceSystemId;
            Token = token;
        }

        /// <summary>
        /// Gets the input stream containing command arguments and options.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown if the command context has been disposed.</exception>
        public Stream InputStream => inputStream ?? throw new ObjectDisposedException(nameof(CommandContext));

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
        public string SourceSystemId { get; }

        /// <summary>
        /// Gets the token of the user who is executing the command.  In order for the token to be passed to the command context,
        /// the command must have the 'token' scope.
        /// </summary>
        public string? Token { get; }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (inputStream != null)
            {
                await inputStream.DisposeAsync();
                inputStream = null;
            }
        }
    }
}
