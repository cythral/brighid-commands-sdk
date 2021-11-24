using System.Collections.Generic;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <summary>
    /// Analyzer that gets a command's parameters.
    /// </summary>
    public interface ICommandParameterAnalyzer
    {
        /// <summary>
        /// Gets the parameters for a command.
        /// </summary>
        /// <param name="commandContext">The context of the command to get parameters for.</param>
        /// <returns>The command's parameters.</returns>
        IEnumerable<CommandParameter> GetCommandParameters(CommandContext commandContext);
    }
}
