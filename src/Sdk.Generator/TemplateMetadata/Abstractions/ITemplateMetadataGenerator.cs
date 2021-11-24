using System.Collections.Generic;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <summary>
    /// Generator that generates CloudFormation templates for Brighid Commands.
    /// </summary>
    public interface ITemplateMetadataGenerator
    {
        /// <summary>
        /// Generates a CloudFormation template for the given command contexts.
        /// </summary>
        /// <param name="commandContexts">A collection of command contexts to generate a template for.</param>
        /// <returns>The resulting template.</returns>
        string GenerateTemplateMetadata(IEnumerable<CommandContext> commandContexts);
    }
}
