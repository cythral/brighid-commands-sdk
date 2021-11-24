using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using Brighid.Commands.Sdk.Models;
using Brighid.Commands.Sdk.Models.CloudFormation;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using YamlDotNet.Serialization;

using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk.PackageTool
{
    public class TemplateServiceTests
    {
        [TestFixture]
        [Category("Unit")]
        public class GenerateTemplateTests
        {
            [Test, Auto]
            public async Task ShouldGenerateTemplateWithCommandName(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.Name.Should().Be(command.Name);
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithCommandDescription(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.Description.Should().Be(command.Description);
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithEmbeddedType(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.Type.Should().Be(CommandType.Embedded);
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithRequiredRole(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.RequiredRole.Should().Be(command.RequiredRole);
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithParameters(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.Parameters.Should().BeEquivalentTo(command.Parameters);
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithIsEnabled(
                CommandMetadata command,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.IsEnabled.Should().BeTrue();
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateWithEmbeddedLocation(
                CommandMetadata command,
                [Frozen] EmbeddedCommandLocation location,
                [Frozen, Substitute] ICommandService commandService,
                [Target] TemplateService templateService,
                CancellationToken cancellationToken
            )
            {
                using var stream = new MemoryStream();
                using var reader = new StreamReader(stream);
                await templateService.GenerateTemplate(stream, new[] { command }, cancellationToken);
                stream.Position = 0;

                var serializer = new DeserializerBuilder().Build();
                var result = serializer.Deserialize<Template>(reader);

                var resource = result!.Resources.ElementAt(0).Value;
                resource.Properties.EmbeddedLocation.Should().BeEquivalentTo(location);

                await commandService.Received().PackageArtifact(Is(command), Is(cancellationToken));
            }
        }
    }
}
