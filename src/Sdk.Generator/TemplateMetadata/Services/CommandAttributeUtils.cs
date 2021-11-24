using System.Linq;
using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <inheritdoc />
    public class CommandAttributeUtils : ICommandAttributeUtils
    {
        /// <inheritdoc />
        public string GetCommandName(AttributeData attributeData)
        {
            return (string)attributeData.ConstructorArguments[0].Value!;
        }

        /// <inheritdoc />
        public string? GetCommandDescription(AttributeData attributeData)
        {
            return GetNamedArgumentValue<string>(attributeData, "Description");
        }

        /// <inheritdoc />
        public string? GetCommandRequiredRole(AttributeData attributeData)
        {
            return GetNamedArgumentValue<string>(attributeData, "RequiredRole");
        }

        /// <inheritdoc />
        public string? GetCommandStartupTypeName(AttributeData attributeData)
        {
            var typeSymbol = GetNamedArgumentValue<INamedTypeSymbol>(attributeData, "StartupType");
            return typeSymbol?.ToDisplayString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TType? GetNamedArgumentValue<TType>(AttributeData attributeData, string argumentName)
        {
            var query = from argument in attributeData.NamedArguments
                        where argument.Key == argumentName
                        select (TType?)argument.Value.Value;

            return query.FirstOrDefault();
        }
    }
}
