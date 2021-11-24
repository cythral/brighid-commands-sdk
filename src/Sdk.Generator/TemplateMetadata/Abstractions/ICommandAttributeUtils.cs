using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <summary>
    /// Utilities for working with command attributes.
    /// </summary>
    public interface ICommandAttributeUtils
    {
        /// <summary>
        /// Gets the command name from the given attribute.
        /// </summary>
        /// <param name="attributeData">The attribute to get a command name from.</param>
        /// <returns>The command's name.</returns>
        string GetCommandName(AttributeData attributeData);

        /// <summary>
        /// Gets the command description from the given attribute.
        /// </summary>
        /// <param name="attributeData">The attribute to get a command description from.</param>
        /// <returns>The command's description, or null if there is none.</returns>
        string? GetCommandDescription(AttributeData attributeData);

        /// <summary>
        /// Gets the command's required role from the given attribute.
        /// </summary>
        /// <param name="attributeData">The attribute to get a command's required role from.</param>
        /// <returns>The command's required role, or null if there is none.</returns>
        string? GetCommandRequiredRole(AttributeData attributeData);

        /// <summary>
        /// Gets the name of the command's startup type.
        /// </summary>
        /// <param name="attributeData">The attribute to get a command's startup type name for.</param>
        /// <returns>The name of the command's startup type, or null if there is none.</returns>
        string? GetCommandStartupTypeName(AttributeData attributeData);
    }
}
