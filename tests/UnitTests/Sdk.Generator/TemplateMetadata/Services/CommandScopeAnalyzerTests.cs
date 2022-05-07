using System;
using System.Linq;
using System.Reflection;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    public class CommandScopeAnalyzerTests
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
                [RequiresScope(""token"")]
                [RequiresScope(""debug"")]
                class TestCommand : ICommand<TestParameter>
                {
                    public Task<CommandResult> Run(CommandContext<TestParameter> context, CancellationToken cancellationToken = default)
                    {{
                        throw new NotImplementedException();
                    }}
                }

                class TestParameter
                {
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
        public void ScopesShouldContainToken(
            [Target] CommandScopeAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var scopes = analyzer.GetCommandScopes(context);

            scopes.Should().Contain("token");
        }

        [Test, Auto]
        public void ScopesShouldContainDebug(
            [Target] CommandScopeAnalyzer analyzer
        )
        {
            var context = CreateCommandContext();
            var scopes = analyzer.GetCommandScopes(context);

            scopes.Should().Contain("debug");
        }
    }
}
