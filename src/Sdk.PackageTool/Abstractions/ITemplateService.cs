using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Service for working with CloudFormation templates.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// Generates a CloudFormation template for a set of commands.
        /// </summary>
        /// <param name="stream">The stream to write the template to.</param>
        /// <param name="metadata">Metadata about commands to generate a CloudFormation template for.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting template.</returns>
        Task GenerateTemplate(Stream stream, CommandMetadata[] metadata, CancellationToken cancellationToken = default);
    }
}
