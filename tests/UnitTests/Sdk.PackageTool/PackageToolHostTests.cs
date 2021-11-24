using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using Brighid.Commands.Sdk.Models;

using Microsoft.Extensions.Hosting;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk.PackageTool
{
    public class PackageToolHostTests
    {
        [TestFixture]
        [Category("Unit")]
        public class StartAsyncTests
        {
            [Test, Auto]
            public async Task ShouldSetExitCodeTo1IfArgumentNotPresent(
                [Frozen, Substitute] IEnvironmentService environment,
                [Target] PackageToolHost host
            )
            {
                environment.GetCommandLineArguments().Returns(Array.Empty<string>());

                await host.StartAsync();

                environment.Received().ExitCode = 1;
            }

            [Test, Auto]
            public async Task ShouldSetExitCodeTo1IfArgument1IsNotAValidFile(
                string file,
                [Frozen, Substitute] IFileService fileService,
                [Frozen, Substitute] IEnvironmentService environment,
                [Target] PackageToolHost host
            )
            {
                fileService.Exists(Any<string>()).Returns(false);
                environment.GetCommandLineArguments().Returns(new[] { string.Empty, file });

                await host.StartAsync();

                environment.Received().ExitCode = 1;
                fileService.Received().Exists(Is(file));
            }

            [Test, Auto]
            public async Task ShouldReadInMetadataFile(
                string file,
                [Frozen, Substitute] IFileService fileService,
                [Frozen, Substitute] IEnvironmentService environment,
                [Target] PackageToolHost host,
                CancellationToken cancellationToken
            )
            {
                environment.GetCommandLineArguments().Returns(new[] { string.Empty, file });

                await host.StartAsync(cancellationToken);

                await fileService.Received().ReadCommandMetadata(Is(file), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldGenerateTemplateFromMetadata(
                string file,
                string temporaryDestination,
                CommandMetadata[] metadata,
                [Frozen] CommandLineOptions options,
                [Frozen] Stream stream,
                [Frozen, Substitute] IFileService fileService,
                [Frozen, Substitute] ITemplateService templateService,
                [Frozen, Substitute] IEnvironmentService environment,
                [Target] PackageToolHost host,
                CancellationToken cancellationToken
            )
            {
                fileService.GenerateTemporaryFilePath().Returns(temporaryDestination);
                fileService.ReadCommandMetadata(Any<string>(), Any<CancellationToken>()).Returns(metadata);
                environment.GetCommandLineArguments().Returns(new[] { string.Empty, file });

                await host.StartAsync(cancellationToken);

                fileService.Received().OpenWrite(Is(temporaryDestination));
                await templateService.Received().GenerateTemplate(Is(stream), Is(metadata), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldMoveTemporaryFileToTemplateDestination(
                string file,
                string temporaryDestination,
                CommandMetadata[] metadata,
                [Frozen] CommandLineOptions options,
                [Frozen] Stream stream,
                [Frozen, Substitute] IFileService fileService,
                [Frozen, Substitute] ITemplateService templateService,
                [Frozen, Substitute] IEnvironmentService environment,
                [Target] PackageToolHost host,
                CancellationToken cancellationToken
            )
            {
                fileService.GenerateTemporaryFilePath().Returns(temporaryDestination);
                fileService.ReadCommandMetadata(Any<string>(), Any<CancellationToken>()).Returns(metadata);
                environment.GetCommandLineArguments().Returns(new[] { string.Empty, file });

                await host.StartAsync(cancellationToken);

                fileService.Received().Move(Is(temporaryDestination), Is(options.TemplateDestination));
            }

            [Test, Auto]
            public async Task ShouldShutDownApplication(
                [Frozen, Substitute] IHostApplicationLifetime lifetime,
                [Target] PackageToolHost host
            )
            {
                await host.StartAsync();

                lifetime.Received().StopApplication();
            }
        }
    }
}
