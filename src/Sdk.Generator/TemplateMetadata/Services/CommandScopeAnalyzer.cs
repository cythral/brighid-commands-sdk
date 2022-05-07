using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <inheritdoc />
    public class CommandScopeAnalyzer : ICommandScopeAnalyzer
    {
        /// <inheritdoc />
        public IEnumerable<string> GetCommandScopes(CommandContext context)
        {
            var symbol = context.SemanticModel.GetDeclaredSymbol(context.ClassDeclaration);
            var attributes = symbol?.GetAttributes() ?? ImmutableArray.Create<AttributeData>();

            return from attribute in attributes
                   where attribute.AttributeClass?.ToDisplayString() == "Brighid.Commands.Sdk.RequiresScopeAttribute"
                   select (string)attribute.ConstructorArguments[0].Value!;
        }
    }
}
