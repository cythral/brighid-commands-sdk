using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Syntax receiver that records class syntax nodes with a given attribute.
    /// </summary>
    public class ClassSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly List<CommandContext> results = new();

        /// <summary>
        /// Gets the resulting contexts.
        /// </summary>
        public IEnumerable<CommandContext> Results => results;

        /// <inheritdoc />
        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax classSyntaxNode)
            {
                return;
            }

            var attributes = context.SemanticModel.GetDeclaredSymbol(classSyntaxNode)?.GetAttributes() ?? ImmutableArray.Create<AttributeData>();
            var query = from attr in attributes
                        let attrClass = attr.AttributeClass
                        where attrClass.ToDisplayString() == "Brighid.Commands.Sdk.CommandAttribute"
                        select attr;

            if (query.Any())
            {
                results.Add(new CommandContext(
                    tree: classSyntaxNode.SyntaxTree,
                    semanticModel: context.SemanticModel,
                    attributeData: query.First(),
                    classDeclaration: classSyntaxNode
                ));
            }
        }
    }
}
