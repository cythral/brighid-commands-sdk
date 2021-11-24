using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Brighid.Commands.Sdk.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <inheritdoc />
    /// <todo>Handle case when command does not have run method.</todo>
    /// <todo>Handle case when parameter is neither a string nor numeric.</todo>
    public class CommandParameterAnalyzer : ICommandParameterAnalyzer
    {
        /// <inheritdoc />
        public IEnumerable<CommandParameter> GetCommandParameters(CommandContext commandContext)
        {
            var runMethod = GetRunMethod(commandContext);
            var type = GetParameterType(commandContext.SemanticModel, runMethod);

            foreach (var property in type.GetMembers().OfType<IPropertySymbol>())
            {
                var attributes = property.GetAttributes();
                foreach (var attribute in attributes)
                {
                    var parameter = attribute.AttributeClass?.ToDisplayString() switch
                    {
                        "Brighid.Commands.Sdk.ArgumentAttribute" => GetCommandParameter(attribute, property, true),
                        "Brighid.Commands.Sdk.OptionAttribute" => GetCommandParameter(attribute, property, false),
                        _ => (CommandParameter?)null,
                    };

                    if (parameter.HasValue)
                    {
                        yield return parameter.Value;
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CommandParameter GetCommandParameter(AttributeData attribute, IPropertySymbol property, bool isArgument)
        {
            var parameterName = property.Name;
            var description = GetNamedArgument<string>(attribute, "Description");
            var parameterType = GetCommandParameterType(property);
            var argumentIndex = isArgument
                ? (byte?)attribute.ConstructorArguments[0].Value
                : null;

            return new CommandParameter(
                parameterName,
                description,
                parameterType,
                argumentIndex
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CommandParameterType GetCommandParameterType(IPropertySymbol property)
        {
            return property.Type.ToDisplayString() == "string" ? CommandParameterType.String : CommandParameterType.Number;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static TType? GetNamedArgument<TType>(AttributeData attributeData, string argumentName)
        {
            var query = from argument in attributeData.NamedArguments
                        where argument.Key == argumentName
                        select (TType?)argument.Value.Value;

            return query.FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static MethodDeclarationSyntax GetRunMethod(CommandContext commandContext)
        {
            var query = from member in commandContext.ClassDeclaration.Members
                        where member is MethodDeclarationSyntax method && method.Identifier.Text == "Run"
                        select (MethodDeclarationSyntax)member;

            return query.First();
        }

        /// <summary>
        /// Gets the parameter type from the Run method.  This corresponds to the generic type argument in CommandContext{TParameterType}.
        /// </summary>
        /// <param name="semanticModel">The semantic model of the syntax tree being analyzed.</param>
        /// <param name="method">The run method to get a parameter type for.</param>
        /// <returns>The type symbol of the type parameter.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static INamedTypeSymbol GetParameterType(SemanticModel semanticModel, MethodDeclarationSyntax method)
        {
            var parameter = method.ParameterList.Parameters[0].Type!;
            var type = (INamedTypeSymbol)semanticModel.GetTypeInfo(parameter).Type!;
            return (INamedTypeSymbol)type.TypeArguments[0];
        }
    }
}
