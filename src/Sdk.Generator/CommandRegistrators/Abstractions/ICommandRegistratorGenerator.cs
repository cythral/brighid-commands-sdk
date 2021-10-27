using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brighid.Commands.Sdk.Generator.CommandRegistrators
{
    /// <summary>
    /// Generator which generates command runners.
    /// </summary>
    public interface ICommandRegistratorGenerator
    {
        /// <summary>
        /// Generate a command runner.
        /// </summary>
        /// <param name="context">Context holder for this pass of the generator.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The resulting command runner.</returns>
        Task<CompilationUnitSyntax> Generate(CommandContext context, CancellationToken cancellationToken);
    }
}
