using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Generator.CommandRegistrators;
using Brighid.Commands.Sdk.Generator.TemplateMetadata;

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Host that runs the source generator.
    /// </summary>
    public class SourceGeneratorHost : IHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceGeneratorHost" /> class.
        /// </summary>
        /// <param name="services">The service provider containing the host's services.</param>
        public SourceGeneratorHost(IServiceProvider services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the host's service provider.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var receiver = Services.GetRequiredService<ClassSyntaxReceiver>();
            var context = Services.GetRequiredService<GeneratorContext>();
            var registratorGenerator = Services.GetRequiredService<ICommandRegistratorGenerator>();
            var templateMetadataGenerator = Services.GetRequiredService<ITemplateMetadataGenerator>();
            var syntaxUtils = Services.GetRequiredService<ISyntaxUtils>();

            foreach (var result in receiver.Results)
            {
                var unit = await registratorGenerator.Generate(result, cancellationToken);
                var hint = $"{result.ClassDeclaration.Identifier.Text}Registrator";
                var source = unit.NormalizeWhitespace().GetText(Encoding.UTF8);
                context.ExecutionContext.AddSource(hint, source);
            }

            var intermediateOutputPath = syntaxUtils.GetBuildProperty("IntermediateOutputPath");
            var metadata = templateMetadataGenerator.GenerateTemplateMetadata(receiver.Results);
            File.WriteAllText($"{intermediateOutputPath}/TemplateMetadata.json", metadata);

            var lifetime = Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.StopApplication();
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
