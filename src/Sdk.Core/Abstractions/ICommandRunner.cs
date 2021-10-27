using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Service that wraps a command, providing the ability to invoke a command.
    /// </summary>
    public interface ICommandRunner
    {
        /// <summary>
        /// Runs the command.
        /// </summary>
        /// <param name="context">The context to use when running the command.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>Reply to send back to the user.</returns>
        Task<CommandResult> Run(CommandContext context, CancellationToken cancellationToken = default);
    }
}
