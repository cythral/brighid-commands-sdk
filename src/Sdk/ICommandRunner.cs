using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Service that runs a command.
    /// </summary>
    /// <typeparam name="TRequest">The type of request the command takes.</typeparam>
    public interface ICommandRunner<in TRequest>
    {
        /// <summary>
        /// Runs the command.
        /// </summary>
        /// <param name="context">The context to use when running the command.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>Reply to send back to the user.</returns>
        Task<string> Run(TRequest context, CancellationToken cancellationToken = default);
    }
}
