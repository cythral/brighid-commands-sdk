using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using Brighid.Commands.Sdk.Models;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    public class TemplateMetadataGeneratorTests
    {
        [Test, Auto]
        public void ShouldAddNameMetadata(
            string name,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            commandAttributeUtils.GetCommandName(Any<AttributeData>()).Returns(name);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].Name.Should().Be(name);
        }

        [Test, Auto]
        public void ShouldAddTypeNameMetadata(
            string typeName,
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            syntaxUtils.GetFullTypeName(Any<ClassDeclarationSyntax>()).Returns(typeName);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].TypeName.Should().Be(typeName + "Registrator");
        }

        [Test, Auto]
        public void ShouldAddAssemblyNameMetadata(
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].AssemblyName.Should().Be(commandContext.SemanticModel.Compilation.AssemblyName);
        }

        [Test, Auto]
        public void ShouldAddIntermediateOutputPathMetadata(
            string intermediateOutputPath,
            GeneratorContext generatorContext,
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            syntaxUtils.GetBuildProperty(Is("IntermediateOutputPath")).Returns(intermediateOutputPath);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].IntermediateOutputPath.Should().Be(intermediateOutputPath);
        }

        [Test, Auto]
        public void ShouldThrowIfIntermediateOutputPathIsNull(
            string intermediateOutputPath,
            GeneratorContext generatorContext,
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            syntaxUtils.GetBuildProperty(Is("IntermediateOutputPath")).Returns((string?)null);

            Func<string> func = () => generator.GenerateTemplateMetadata(new[] { commandContext });

            var exception = func.Should().Throw<MissingIntermediateOutputPathException>().Which;
        }

        [Test, Auto]
        public void ShouldAddOutputPathMetadata(
            string outputPath,
            GeneratorContext generatorContext,
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            syntaxUtils.GetBuildProperty(Is("PublishDir")).Returns(outputPath);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].OutputPath.Should().Be(outputPath);
        }

        [Test, Auto]
        public void ShouldThrowIfOutputPathIsNull(
            string outputPath,
            GeneratorContext generatorContext,
            CommandContext commandContext,
            [Frozen, Substitute] ISyntaxUtils syntaxUtils,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            syntaxUtils.GetBuildProperty(Is("PublishDir")).Returns((string?)null);

            Func<string> func = () => generator.GenerateTemplateMetadata(new[] { commandContext });

            var exception = func.Should().Throw<MissingOutputPathException>().Which;
        }

        [Test, Auto]
        public void ShouldAddDescriptionMetadata(
            string description,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            commandAttributeUtils.GetCommandDescription(Any<AttributeData>()).Returns(description);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].Description.Should().Be(description);
        }

        [Test, Auto]
        public void ShouldAddRequiredRoleMetadata(
            string requiredRole,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator,
            CancellationToken cancellationToken
        )
        {
            commandAttributeUtils.GetCommandRequiredRole(Any<AttributeData>()).Returns(requiredRole);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].RequiredRole.Should().Be(requiredRole);
        }

        [Test, Auto]
        public void ShouldAddStartupTypeMetadata(
            string startupType,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator
        )
        {
            commandAttributeUtils.GetCommandStartupTypeName(Any<AttributeData>()).Returns(startupType);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].StartupType.Should().Be(startupType);
        }

        [Test, Auto]
        public void ShouldAddParametersMetadata(
            IEnumerable<CommandParameter> parameters,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandParameterAnalyzer parameterAnalyzer,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator
        )
        {
            parameterAnalyzer.GetCommandParameters(Any<CommandContext>()).Returns(parameters);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].Parameters.Should().BeEquivalentTo(parameters);
            parameterAnalyzer.Received().GetCommandParameters(Is(commandContext));
        }

        [Test, Auto]
        public void ShouldAddScopesMetadata(
            IEnumerable<string> scopes,
            CommandContext commandContext,
            [Frozen, Substitute] ICommandScopeAnalyzer scopeAnalyzer,
            [Frozen, Substitute] ICommandAttributeUtils commandAttributeUtils,
            [Target] TemplateMetadataGenerator generator
        )
        {
            scopeAnalyzer.GetCommandScopes(Any<CommandContext>()).Returns(scopes);

            var result = generator.GenerateTemplateMetadata(new[] { commandContext });
            var metadatas = JsonSerializer.Deserialize<CommandMetadata[]>(result)!;

            metadatas[0].Scopes.Should().BeEquivalentTo(scopes);
            scopeAnalyzer.Received().GetCommandScopes(Is(commandContext));
        }
    }
}
