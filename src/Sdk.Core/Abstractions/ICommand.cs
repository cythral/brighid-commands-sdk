using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Represents a command that can be run by users from a variety of platforms.
    /// </summary>
    /// <typeparam name="TInput">The type of input to accept in the command.</typeparam>
    public interface ICommand<TInput>
    {
        /// <summary>
        /// Runs a command with the given <paramref name="context" />.
        /// </summary>
        /// <param name="context">The context of the command run.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The result of executing/running the command.</returns>
        Task<CommandResult> Run(CommandContext<TInput> context, CancellationToken cancellationToken = default);
    }
}
