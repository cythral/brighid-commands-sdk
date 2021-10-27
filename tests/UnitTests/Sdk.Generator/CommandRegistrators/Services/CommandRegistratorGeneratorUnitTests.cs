using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NSubstitute;

using NUnit.Framework;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk.Generator.CommandRegistrators
{
    public class CommandRegistratorGeneratorUnitTests
    {
        [TestFixture]
        [Category("Unit")]
        public class GenerateTests
        {
            public static ClassDeclarationSyntax GetClassDeclaration(CompilationUnitSyntax compilation)
            {
                return compilation.DescendantNodes().OfType<ClassDeclarationSyntax>().First();
            }

            public static MethodDeclarationSyntax? GetRegisterMethod(CompilationUnitSyntax compilation)
            {
                var classDeclaration = GetClassDeclaration(compilation);
                var query = from member in classDeclaration.Members
                            where member is MethodDeclarationSyntax method && method.Identifier.Text == "Register"
                            select (MethodDeclarationSyntax)member;

                return query.FirstOrDefault();
            }

            public static NamespaceDeclarationSyntax? GetNamespaceDeclaration(CompilationUnitSyntax compilation)
            {
                return compilation.DescendantNodes().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassSuffixedWithRegistrator(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var classDeclaration = GetClassDeclaration(result);

                classDeclaration.Identifier.Text.Should().Be($"{context.ClassDeclaration.Identifier.Text}Registrator");
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassInTheSameNamespace(
                CommandContext context,
                [Frozen, Substitute] ISyntaxUtils syntaxUtils,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var @namespace = Guid.NewGuid().ToString().Replace('-', '.');
                var expectedNamespaceDeclaration = NamespaceDeclaration(ParseName(@namespace));

                syntaxUtils.GetNamespaceOfClassDeclaration(Any<ClassDeclarationSyntax>()).Returns(expectedNamespaceDeclaration);
                var result = await generator.Generate(context, cancellationToken);
                var namespaceDeclaration = GetNamespaceDeclaration(result);

                namespaceDeclaration!.Name.ToFullString().Should().Be(@namespace);
            }

            [Test, Auto]
            public async Task ShouldHandleClassesWithNoNamespace(
                CommandContext context,
                [Frozen, Substitute] ISyntaxUtils syntaxUtils,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                syntaxUtils.GetNamespaceOfClassDeclaration(Any<ClassDeclarationSyntax>()).Returns((NamespaceDeclarationSyntax?)null);
                var result = await generator.Generate(context, cancellationToken);
                var namespaceDeclaration = GetNamespaceDeclaration(result);
                var classDeclaration = GetClassDeclaration(result);

                namespaceDeclaration.Should().BeNull();
                classDeclaration.Should().NotBeNull();
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatImplementsICommandRegistrator(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var classDeclaration = GetClassDeclaration(result);

                classDeclaration.BaseList.Should().NotBeNull();
                classDeclaration.BaseList!.Types.Should().Contain(type => type.ToFullString() == "Brighid.Commands.Sdk.ICommandRegistrator");
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatIsPublic(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var classDeclaration = GetClassDeclaration(result);

                classDeclaration.Modifiers.Should().Contain(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatContainsARegisterMethod(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var registerMethod = GetRegisterMethod(result);

                registerMethod.Should().NotBeNull();
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatContainsARegisterMethodThatIsPublic(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var registerMethod = GetRegisterMethod(result);

                registerMethod!.Modifiers.Should().Contain(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatContainsARegisterMethodWithCommandRunnerReturnType(
                CommandContext context,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                var result = await generator.Generate(context, cancellationToken);
                var registerMethod = GetRegisterMethod(result);

                registerMethod!.ReturnType.ToFullString().Should().Be("Brighid.Commands.Sdk.ICommandRunner");
            }

            [Test, Auto]
            public async Task ShouldGenerateAClassThatContainsARegisterMethodWithServiceCollectionArgument(
                ParameterSyntax serviceCollectionParameter,
                CommandContext context,
                [Frozen, Substitute] ISyntaxUtils syntaxUtils,
                [Target] CommandRegistratorGenerator generator,
                CancellationToken cancellationToken
            )
            {
                syntaxUtils.CreateParameter(Any<string>(), Any<string>()).Returns(serviceCollectionParameter);

                var result = await generator.Generate(context, cancellationToken);
                var registerMethod = GetRegisterMethod(result);
                var argument = registerMethod!.ParameterList.Parameters.ElementAtOrDefault(0);

                argument!.Identifier.Text.Should().Be(serviceCollectionParameter.Identifier.Text);
                syntaxUtils.Received().CreateParameter(Is("Microsoft.Extensions.DependencyInjection.IServiceCollection"), Is("services"));
            }
        }
    }
}
