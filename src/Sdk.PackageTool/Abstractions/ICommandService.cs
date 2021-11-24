using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Service for working with commands.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// Package a command and upload it to S3.
        /// </summary>
        /// <param name="command">The command to package.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The S3 URL where the command has been uploaded to.</returns>
        Task<EmbeddedCommandLocation> PackageArtifact(CommandMetadata command, CancellationToken cancellationToken = default);
    }
}
