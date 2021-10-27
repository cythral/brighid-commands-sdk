using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// The source generator's context holder.
    /// </summary>
    public class CommandContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext" /> class.
        /// </summary>
        /// <param name="tree">The syntax tree.</param>
        /// <param name="semanticModel">The semantic model.</param>
        /// <param name="attribute">The attribute data associated with the class being analyzed in this context.</param>
        /// <param name="attributeData">The raw attribute data associated with the class being analyzed in this context.</param>
        /// <param name="classDeclaration">The class declaration.</param>
        public CommandContext(
            SyntaxTree tree,
            SemanticModel semanticModel,
            CommandAttribute attribute,
            AttributeData attributeData,
            ClassDeclarationSyntax classDeclaration
        )
        {
            Tree = tree;
            SemanticModel = semanticModel;
            Attribute = attribute;
            AttributeData = attributeData;
            ClassDeclaration = classDeclaration;
        }

        /// <summary>
        /// Gets the syntax tree associated with this context.
        /// </summary>
        public SyntaxTree Tree { get; }

        /// <summary>
        /// Gets the semantic model associated with this context.
        /// </summary>
        public SemanticModel SemanticModel { get; }

        /// <summary>
        /// Gets the attribute data associated with the class being analyzed in this context.
        /// </summary>
        public CommandAttribute Attribute { get; }

        /// <summary>
        /// Gets the raw attribute data associated with the class being analyzed in this context.
        /// </summary>
        public AttributeData AttributeData { get; }

        /// <summary>
        /// Gets the class declaration associated with this context.
        /// </summary>
        public ClassDeclarationSyntax ClassDeclaration { get; }
    }
}
