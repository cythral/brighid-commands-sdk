using System.Linq;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    public class CommandAttributeUtilsTests
    {
        public static AttributeData GetAttributeData(
            string name,
            string description,
            string requiredRole
        )
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@$"
                    using System;
                    using Brighid.Commands.Sdk;

                    namespace Brighid.Parse.Test
                    {{
                        public class CommandAttribute : Attribute
                        {{
                            public CommandAttribute(string name) {{}}

                            public string? Description {{ get; set; }}

                            public string? RequiredRole {{ get; set; }}

                            public Type? StartupType {{ get; set; }}
                        }}

                        [Command(""{name}"", Description = ""{description}"", RequiredRole = ""{requiredRole}"", StartupType = typeof(TestStartup))]
                        public class TestClass
                        {{
                        }}

                        public class TestStartup
                        {{
                        }}
                    }}
                ");

            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("Test")
                .AddReferences(mscorlib)
                .AddSyntaxTrees(syntaxTree);

            var classDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ElementAt(1);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            return semanticModel.GetDeclaredSymbol(classDeclaration)!.GetAttributes().First();
        }

        [TestFixture]
        [Category("Unit")]
        public class GetCommandNameTests
        {
            [Test, Auto]
            public void ShouldReturnTheCommandName(
                string name,
                string description,
                string requiredRole,
                [Target] CommandAttributeUtils commandAttributeUtils
            )
            {
                var attribute = GetAttributeData(name, description, requiredRole);
                var result = commandAttributeUtils.GetCommandName(attribute);
                result.Should().Be(name);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class GetCommandDescriptionTests
        {
            [Test, Auto]
            public void ShouldReturnTheCommandDescription(
                    string name,
                    string description,
                    string requiredRole,
                    [Target] CommandAttributeUtils commandAttributeUtils
                )
            {
                var attribute = GetAttributeData(name, description, requiredRole);
                var result = commandAttributeUtils.GetCommandDescription(attribute);
                result.Should().Be(description);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class GetCommandRequiredRoleTests
        {
            [Test, Auto]
            public void ShouldReturnTheCommandsRequiredRole(
                string name,
                string description,
                string requiredRole,
                [Target] CommandAttributeUtils commandAttributeUtils
            )
            {
                var attribute = GetAttributeData(name, description, requiredRole);
                var result = commandAttributeUtils.GetCommandRequiredRole(attribute);
                result.Should().Be(requiredRole);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class GetCommandStartupTypeNameTests
        {
            [Test, Auto]
            public void ShouldReturnTheCommandsStartupTypeName(
                string name,
                string description,
                string requiredRole,
                [Target] CommandAttributeUtils commandAttributeUtils
            )
            {
                var attribute = GetAttributeData(name, description, requiredRole);
                var result = commandAttributeUtils.GetCommandStartupTypeName(attribute);
                result.Should().Be("Brighid.Parse.Test.TestStartup");
            }
        }
    }
}
