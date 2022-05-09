using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Models;
using Brighid.Commands.Sdk.Models.CloudFormation;

using YamlDotNet.Serialization;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <inheritdoc />
    public class TemplateService : ITemplateService
    {
        private readonly ISerializer serializer;

        private readonly ICommandService commandService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateService" /> class.
        /// </summary>
        /// <param name="commandService">Service for dealing with commands.</param>
        public TemplateService(
            ICommandService commandService
        )
        {
            serializer = new SerializerBuilder()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .WithTypeConverter(new CommandParameterTypeConverter())
                .Build();

            this.commandService = commandService;
        }

        /// <inheritdoc />
        public async Task GenerateTemplate(Stream stream, CommandMetadata[] metadata, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var template = new Template();

            foreach (var commandMetadata in metadata)
            {
                var properties = new CommandResourceProperties
                {
                    Name = commandMetadata.Name,
                    Description = commandMetadata.Description,
                    Type = CommandType.Embedded,
                    RequiredRole = commandMetadata.RequiredRole,
                    Parameters = commandMetadata.Parameters,
                    Scopes = commandMetadata.Scopes,
                    IsEnabled = true,
                    EmbeddedLocation = await commandService.PackageArtifact(commandMetadata, cancellationToken),
                };

                template.Resources.Add($"{commandMetadata.Name}Command", new CommandResource(properties));
            }

            using var writer = new StreamWriter(stream, leaveOpen: true);
            serializer.Serialize(writer, template, typeof(Template));
        }
    }
}
