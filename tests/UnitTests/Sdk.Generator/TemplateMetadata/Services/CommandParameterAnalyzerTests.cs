using System;
using System.Linq;
using System.Reflection;

using Brighid.Commands.Sdk.Models;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    public class CommandParameterAnalyzerTests
    {
        public static readonly SyntaxTree ParameterSyntaxTree = CSharpSyntaxTree.ParseText(@"
            using System;
            using System.Diagnostics;
            using System.Threading;
            using System.Threading.Tasks;

            using Brighid.Commands.Sdk;

            namespace Test
            {
                [Command(""test"")]
                class TestCommand : ICommand<TestParameter>
                {
                    public Task<CommandResult> Run(CommandContext<TestParameter> context, CancellationToken cancellationToken = default)
                    {{
                        throw new NotImplementedException();
                    }}
                }

                class TestParameter
                {
                    [DebuggerDisplay(""Test Debugging Attribute"")]
                    [Argument(0, Description = ""An example argument"")]
                    public string Argument1 { get; set; }

                    [Argument(1, Description = ""Another example argument"")]
                    public int Argument2 { get; set; }

                    [Option(Description = ""An example option"")]
                    public string Option1 { get; set; }
                }
            }
        ");

        public static CSharpCompilation CreateCompilation()
        {
            var runtimeAssembly = Assembly.Load("System.Runtime");
            var corelibAssembly = Assembly.Load("System.Private.CoreLib");
            var runtimeReference = MetadataReference.CreateFromFile(runtimeAssembly.Location);
            var corelibReference = MetadataReference.CreateFromFile(corelibAssembly.Location);
            var attributesReference = MetadataReference.CreateFromFile(typeof(ArgumentAttribute).Assembly.Location);
            var coreReference = MetadataReference.CreateFromFile(typeof(ICommand<>).Assembly.Location);

            return CSharpCompilation.Create("CodeGeneration", new[] { ParameterSyntaxTree }, new[] { runtimeReference, corelibReference, attributesReference, coreReference })
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        }

        public static CommandContext CreateCommandContext()
        {
            var compilation = CreateCompilation();
            Console.WriteLine(string.Join('\n', compilation.GetDiagnostics().Select(diagnostic => diagnostic.GetMessage())));
            var syntaxTree = compilation.SyntaxTrees[0];
            var commandClass = syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var attribute = semanticModel.GetDeclaredSymbol(commandClass)!.GetAttributes()[0]!;

            return new CommandContext(syntaxTree, semanticModel, attribute, commandClass);
        }

        [Test, Auto]
        public void ShouldContain3Parameters(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            parameters.Count().Should().Be(3);
        }

        [Test, Auto]
        public void ShouldContainArgument1ParameterWithName(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            parameters.Should().Contain(parameter => parameter.Name == "Argument1");
        }

        [Test, Auto]
        public void ShouldContainArgument1ParameterWithDescription(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument1").Which;
            parameter.Description.Should().Be("An example argument");
        }

        [Test, Auto]
        public void ShouldContainArgument1ParameterWithIndex(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument1").Which;
            parameter.ArgumentIndex.Should().Be(0);
        }

        [Test, Auto]
        public void ShouldContainArgument1ParameterWithStringType(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument1").Which;
            parameter.Type.Should().Be(CommandParameterType.String);
        }

        [Test, Auto]
        public void ShouldContainArgument2Parameter(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            parameters.Should().Contain(parameter => parameter.Name == "Argument2");
        }

        [Test, Auto]
        public void ShouldContainArgument2ParameterWithDescription(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument2").Which;
            parameter.Description.Should().Be("Another example argument");
        }

        [Test, Auto]
        public void ShouldContainArgument2ParameterWithIndex(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument2").Which;
            parameter.ArgumentIndex.Should().Be(1);
        }

        [Test, Auto]
        public void ShouldContainArgument2ParameterWithNumberParameterType(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Argument2").Which;
            parameter.Type.Should().Be(CommandParameterType.Number);
        }

        [Test, Auto]
        public void ShouldContainOption1Parameter(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            parameters.Should().Contain(parameter => parameter.Name == "Option1");
        }

        [Test, Auto]
        public void ShouldContainOption1ParameterWithDescription(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Option1").Which;
            parameter.Description.Should().Be("An example option");
        }

        [Test, Auto]
        public void ShouldContainOption1ParameterWithStringParameterType(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Option1").Which;
            parameter.Type.Should().Be(CommandParameterType.String);
        }

        [Test, Auto]
        public void ShouldContainOption1ParameterWithNullArgumentIndex(
            [Target] CommandParameterAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var parameters = analyzer.GetCommandParameters(context);

            var parameter = parameters.Should().Contain(parameter => parameter.Name == "Option1").Which;
            parameter.ArgumentIndex.Should().BeNull();
        }
    }
}
