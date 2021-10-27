using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.Generator
{
    public class TypeUtilsTests
    {
        [TestFixture]
        public class GetNamedTypeArgumentTests
        {
            [Test, Auto]
            public void ShouldReturnTheRequestedType(
                [Target] TypeUtils typeUtils
            )
            {
                var attributeSyntaxTree = CSharpSyntaxTree.ParseText(@"
                    using System;

                    namespace TestNamespace
                    {
                        public class TestData
                        {
                        }

                        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
                        public class ExampleAttribute : Attribute
                        {

                            public Type? TestDataType { get; set; }
                        }
                    }
                ");

                var classSyntaxTree = CSharpSyntaxTree.ParseText(@"
                    namespace TestNamespace
                    {
                        [Example(TestDataType = typeof(TestData))]
                        public class TestClass
                        {
                        }
                    }
                ");

                var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                var compilation = CSharpCompilation
                    .Create("ShouldReturnGenericArgumentTest", new[] { attributeSyntaxTree, classSyntaxTree })
                    .AddReferences(mscorlib);

                var classDeclaration = classSyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var semanticModel = compilation.GetSemanticModel(classSyntaxTree);
                var attribute = semanticModel.GetDeclaredSymbol(classDeclaration)!.GetAttributes()[0];
                var result = typeUtils.GetNamedTypeArgument(attribute, "TestDataType");

                result!.ToDisplayString().Should().Be("TestNamespace.TestData");
            }

            [Test, Auto]
            public void ShouldReturnNullIfTheArgumentDoesntExist(
            [Target] TypeUtils typeUtils
        )
            {
                var attributeSyntaxTree = CSharpSyntaxTree.ParseText(@"
                using System;

                namespace TestNamespace
                {
                    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
                    public class ExampleAttribute : Attribute
                    {

                        public Type? TestDataType { get; set; }
                    }
                }
            ");

                var classSyntaxTree = CSharpSyntaxTree.ParseText(@"
                namespace TestNamespace
                {
                    [Example]
                    public class TestClass
                    {
                    }
                }
            ");

                var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                var compilation = CSharpCompilation
                    .Create("ShouldReturnGenericArgumentTest", new[] { attributeSyntaxTree, classSyntaxTree })
                    .AddReferences(mscorlib);

                var classDeclaration = classSyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var semanticModel = compilation.GetSemanticModel(classSyntaxTree);
                var attribute = semanticModel.GetDeclaredSymbol(classDeclaration)!.GetAttributes()[0];
                var result = typeUtils.GetNamedTypeArgument(attribute, "TestDataType");

                result.Should().BeNull();
            }
        }
    }
}
