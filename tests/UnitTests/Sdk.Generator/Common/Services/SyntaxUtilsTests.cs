using System;
using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.Generator
{
    public class SyntaxUtilsTests
    {
        [TestFixture]
        public class GetNamespaceOfClassDeclarationTests
        {
            [Test, Auto]
            public void ShouldReturnTheNamespaceIfItsPresent(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
                    namespace Brighid.Parse.Test
                    {
                        public class TestClass
                        {
                        }
                    }
                ");

                var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var result = syntaxUtils.GetNamespaceOfClassDeclaration(classDeclaration);

                result.Should().NotBeNull();
                result!.Name.ToString().Should().Be("Brighid.Parse.Test");
            }

            [Test, Auto]
            public void ShouldReturnNullIfNotPresent(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
                    public class TestClass
                    {
                    }
                ");

                var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var result = syntaxUtils.GetNamespaceOfClassDeclaration(classDeclaration);

                result.Should().BeNull();
            }
        }

        [TestFixture]
        public class GetFullTypeNameTests
        {
            [Test, Auto]
            public void ShouldReturnTheFullTypeName(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
                    namespace A.B.C
                    {
                        public class D
                        {
                        }
                    }
                ");

                var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var result = syntaxUtils.GetFullTypeName(classDeclaration);

                result.Should().Be("A.B.C.D");
            }

            [Test, Auto]
            public void ShouldReturnClassNameIfNotInNamespace(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
                    public class D
                    {
                    }
                ");

                var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var result = syntaxUtils.GetFullTypeName(classDeclaration);

                result.Should().Be("D");
            }
        }

        [TestFixture]
        public class CreateParameterTests
        {
            [Test, Auto]
            public void ShouldCreateParameterWithTheGivenName(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var typeName = Guid.NewGuid().ToString().Replace('-', '.');
                var parameterName = "parameterName";

                var result = syntaxUtils.CreateParameter(typeName, parameterName);

                result.Identifier.Text.Should().Be(parameterName);
            }

            [Test, Auto]
            public void ShouldCreateParameterWithTheGivenType(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var typeName = Guid.NewGuid().ToString().Replace('-', '.');
                var parameterName = "parameterName";

                var result = syntaxUtils.CreateParameter(typeName, parameterName);

                result.Type!.ToFullString().Should().Be(typeName);
            }
        }

        [TestFixture]
        public class GetImplementedBaseTypeArgumentName
        {
            [Test, Auto]
            public void ShouldReturnTheGenericArgumentName(
                [Target] SyntaxUtils syntaxUtils
            )
            {
                var interfaceSyntaxTree = CSharpSyntaxTree.ParseText(@"
                    namespace Brighid.Commands.Sdk
                    {
                        public interface ICommand<TInput>
                        {
                            void Foo(TInput input);
                        }
                    }
                ");

                var classInputSyntaxTree = CSharpSyntaxTree.ParseText(@"
                    namespace Test
                    {
                        public class TestClassInput
                        {
                        }
                    }
                ");

                var classSyntaxTree = CSharpSyntaxTree.ParseText(@"
                    using Brighid.Commands.Sdk;
                    using Test;

                    public class TestClass : ICommand<TestClassInput>
                    {
                        public void Foo(TestClassInput input)
                        {
                        }
                    }
                ");

                var compilation = CSharpCompilation.Create("ShouldReturnGenericArgumentTest", new[] { interfaceSyntaxTree, classInputSyntaxTree, classSyntaxTree });
                var classDeclaration = classSyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var semanticModel = compilation.GetSemanticModel(classSyntaxTree);
                var result = syntaxUtils.GetImplementedBaseTypeArgumentName(semanticModel, classDeclaration, "Brighid.Commands.Sdk.ICommand", 0);

                result.Should().Be("Test.TestClassInput");
            }
        }
    }
}
