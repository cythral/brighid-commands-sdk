using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Service for interacting with files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Checks if a file exists.
        /// </summary>
        /// <param name="file">The file to check existence for.</param>
        /// <returns>True if the file exists, or false if not.</returns>
        bool Exists(string file);

        /// <summary>
        /// Generates a temporary file path.
        /// </summary>
        /// <returns>The path to the temporary file.</returns>
        string GenerateTemporaryFilePath();

        /// <summary>
        /// Opens a file for writing.
        /// </summary>
        /// <param name="file">The file to open a write stream for.</param>
        /// <returns>The resulting file write stream.</returns>
        Stream OpenWrite(string file);

        /// <summary>
        /// Moves a file from one place to another.
        /// </summary>
        /// <param name="source">The source file.</param>
        /// <param name="destination">The destination to move the file to.</param>
        void Move(string source, string destination);

        /// <summary>
        /// Zips a directory into a temporary zip file and returns the filename.
        /// </summary>
        /// <param name="directory">The directory to zip.</param>
        /// <returns>The name of the zip file created.</returns>
        string CreateZip(string directory);

        /// <summary>
        /// Compute the SHA256 checksum of the given file.
        /// </summary>
        /// <param name="file">The file to compute a checksum for.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The sha256 checksum of the given file.</returns>
        Task<string> Sha256sum(string file, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads command metadata from a file.
        /// </summary>
        /// <param name="file">The file to read metadata from.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting command metadata.</returns>
        Task<CommandMetadata[]> ReadCommandMetadata(string file, CancellationToken cancellationToken);
    }
}
