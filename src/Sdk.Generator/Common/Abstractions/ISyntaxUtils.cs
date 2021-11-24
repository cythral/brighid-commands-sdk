using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Utilities for dealing with syntax nodes and declarations.
    /// </summary>
    public interface ISyntaxUtils
    {
        /// <summary>
        /// Gets the value of a build property.
        /// </summary>
        /// <param name="name">The name of the build property to get a value for.</param>
        /// <returns>The value of the build property, or null if it wasn't found.</returns>
        string? GetBuildProperty(string name);

        /// <summary>
        /// Gets the namespace of a class declaration.
        /// </summary>
        /// <param name="classDeclaration">The class declaration to get a namespace for.</param>
        /// <returns>The namespace of the class declaration, or null if the class does not have a namespace.</returns>
        NamespaceDeclarationSyntax? GetNamespaceOfClassDeclaration(ClassDeclarationSyntax classDeclaration);

        /// <summary>
        /// Gets the full type name of a class declaration.
        /// </summary>
        /// <param name="classDeclaration">The class declaration to get a full name for.</param>
        /// <returns>The full name, including the namespace of the given class declaration.</returns>
        string GetFullTypeName(ClassDeclarationSyntax classDeclaration);

        /// <summary>
        /// Creates a parameter with the given type and name.
        /// </summary>
        /// <param name="typeName">The name of the type to use for the parameter.</param>
        /// <param name="parameterName">The name of the parameter to create.</param>
        /// <returns>The resulting parameter syntax.</returns>
        ParameterSyntax CreateParameter(string typeName, string parameterName);

        /// <summary>
        /// Gets the name of a generic argument specified within an implemented base type. For example if a
        /// class implements ICommand{TestCommandInput}, then this returns TestCommandInput.
        /// </summary>
        /// <param name="semanticModel">The semantic model to lookup types on the class with.</param>
        /// <param name="classDeclaration">The class to search for an implemented base type for.</param>
        /// <param name="baseTypeName">The name of the base type to get an argument name for.</param>
        /// <param name="argument">The argument number (starting from 0) to retrieve from the base type.</param>
        /// <returns>The name of the generic argument in the base type, or null if it wasn't found.</returns>
        string? GetImplementedBaseTypeArgumentName(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, string baseTypeName, int argument);
    }
}
