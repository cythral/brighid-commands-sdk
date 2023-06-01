using System.Collections.Generic;
using System.Text.Json;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.Generator.TemplateMetadata
{
    /// <inheritdoc />
    public class TemplateMetadataGenerator : ITemplateMetadataGenerator
    {
        private readonly ICommandAttributeUtils commandAttributeUtils;
        private readonly ICommandParameterAnalyzer parameterAnalyzer;
        private readonly ICommandScopeAnalyzer scopeAnalyzer;
        private readonly ISyntaxUtils syntaxUtils;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMetadataGenerator" /> class.
        /// </summary>
        /// <param name="commandAttributeUtils">Utilities for working with command attributes.</param>
        /// <param name="parameterAnalyzer">The Command Parameter Analyzer.</param>
        /// <param name="scopeAnalyzer">Analyzer capable of retrieving scopes.</param>
        /// <param name="syntaxUtils">Utilities for working with roslyn syntaxes.</param>
        public TemplateMetadataGenerator(
            ICommandAttributeUtils commandAttributeUtils,
            ICommandParameterAnalyzer parameterAnalyzer,
            ICommandScopeAnalyzer scopeAnalyzer,
            ISyntaxUtils syntaxUtils
        )
        {
            this.commandAttributeUtils = commandAttributeUtils;
            this.parameterAnalyzer = parameterAnalyzer;
            this.scopeAnalyzer = scopeAnalyzer;
            this.syntaxUtils = syntaxUtils;
        }

        /// <inheritdoc />
        public string GenerateTemplateMetadata(IEnumerable<CommandContext> commandContexts)
        {
            var metadatas = GenerateMetadatas(commandContexts);
            return JsonSerializer.Serialize(metadatas);
        }

        private IEnumerable<CommandMetadata> GenerateMetadatas(IEnumerable<CommandContext> commandContexts)
        {
            foreach (var context in commandContexts)
            {
                var name = commandAttributeUtils.GetCommandName(context.AttributeData);
                var typeName = syntaxUtils.GetFullTypeName(context.ClassDeclaration) + "Registrator";
                var assemblyName = context.SemanticModel.Compilation.AssemblyName ?? string.Empty;
                var intermediateOutputPath = syntaxUtils.GetBuildProperty("IntermediateOutputPath") ?? throw new MissingIntermediateOutputPathException();
                var outputPath = syntaxUtils.GetBuildProperty("OutputPath") ?? throw new MissingOutputPathException();
                var description = commandAttributeUtils.GetCommandDescription(context.AttributeData);
                var requiredRole = commandAttributeUtils.GetCommandRequiredRole(context.AttributeData);
                var startupType = commandAttributeUtils.GetCommandStartupTypeName(context.AttributeData);
                var parameters = parameterAnalyzer.GetCommandParameters(context);
                var scopes = scopeAnalyzer.GetCommandScopes(context);

                yield return new CommandMetadata(
                    name,
                    typeName,
                    assemblyName,
                    intermediateOutputPath,
                    outputPath,
                    parameters,
                    scopes
                )
                {
                    Description = description,
                    RequiredRole = requiredRole,
                    StartupType = startupType,
                };
            }
        }
    }
}
