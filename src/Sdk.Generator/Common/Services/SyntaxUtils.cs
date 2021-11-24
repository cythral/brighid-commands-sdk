using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brighid.Commands.Sdk.Generator
{
    /// <inheritdoc />
    public class SyntaxUtils : ISyntaxUtils
    {
        private readonly GeneratorContext generatorContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxUtils" /> class.
        /// </summary>
        /// <param name="generatorContext">The source generator execution context.</param>
        public SyntaxUtils(
            GeneratorContext generatorContext
        )
        {
            this.generatorContext = generatorContext;
        }

        /// <inheritdoc />
        public string? GetBuildProperty(string name)
        {
            generatorContext.ExecutionContext.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.{name}", out var value);
            return value;
        }

        /// <inheritdoc />
        public NamespaceDeclarationSyntax? GetNamespaceOfClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            var declaration = classDeclaration.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();

            return declaration != null
                ? NamespaceDeclaration(declaration.Name)
                : null;
        }

        /// <inheritdoc />
        public string GetFullTypeName(ClassDeclarationSyntax classDeclaration)
        {
            var @namespace = GetNamespaceOfClassDeclaration(classDeclaration);
            return @namespace != null
                ? $"{@namespace.Name.ToFullString().Trim()}.{classDeclaration.Identifier.Text}"
                : classDeclaration.Identifier.Text;
        }

        /// <inheritdoc />
        public ParameterSyntax CreateParameter(string typeName, string parameterName)
        {
            return Parameter(ParseToken(parameterName))
                .WithType(ParseTypeName(typeName));
        }

        /// <inheritdoc />
        public string? GetImplementedBaseTypeArgumentName(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, string baseTypeName, int argument)
        {
            var typeDisplayFormat = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.None,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.None
            );

            var inputType = (from typeSyntax in classDeclaration.BaseList?.Types.ToArray() ?? Array.Empty<BaseTypeSyntax>()
                             let type = semanticModel.GetTypeInfo(typeSyntax.Type).ConvertedType
                             where type.ToDisplayString(typeDisplayFormat) == baseTypeName
                             select ((INamedTypeSymbol)type).TypeArguments[argument]).First();

            var inputTypeDisplayFormat = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.None
            );

            return inputType.ToDisplayString(inputTypeDisplayFormat);
        }
    }
}
