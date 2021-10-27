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
        private readonly ITypeUtils typeUtils;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassSyntaxReceiver" /> class.
        /// </summary>
        /// <param name="typeUtils">Utilities for interacting with types and symbols.</param>
        public ClassSyntaxReceiver(
            ITypeUtils typeUtils
        )
        {
            this.typeUtils = typeUtils;
        }

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
                        where typeUtils.IsSymbolEqualToType(attrClass, typeof(CommandAttribute))
                        select (attr, typeUtils.LoadAttributeData<CommandAttribute>(attr));

            if (query.Any())
            {
                var (attributeData, attribute) = query.First();

                results.Add(new CommandContext(
                    tree: classSyntaxNode.SyntaxTree,
                    semanticModel: context.SemanticModel,
                    attribute: attribute,
                    attributeData: attributeData,
                    classDeclaration: classSyntaxNode
                ));
            }
        }
    }
}
