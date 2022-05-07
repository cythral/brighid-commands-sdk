using System.Collections.Generic;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <summary>
    /// Analyzer that extracts command scopes from a class declaration.
    /// </summary>
    public interface ICommandScopeAnalyzer
    {
        /// <summary>
        /// Gets command scopes for a class declaration.
        /// </summary>
        /// <param name="context">The command context to get scopes for.</param>
        /// <returns>A list of command scopes.</returns>
        IEnumerable<string> GetCommandScopes(CommandContext context);
    }
}
