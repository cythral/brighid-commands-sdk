using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using Brighid.Commands.Sdk.Models;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk.PackageTool
{
    public class CommandServiceTests
    {
        [TestFixture]
        [Category("Unit")]
        public class PackageArtifactTests
        {
            [Test, Auto]
            public async Task ShouldCreateZipOfOutputDirectory(
                CommandMetadata command,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                await service.PackageArtifact(command, cancellationToken);

                file.Received().CreateZip(Is(command.OutputPath));
            }

            [Test, Auto]
            public async Task ShouldUploadZipToS3(
                string zipFile,
                string checksum,
                CommandMetadata command,
                [Frozen] CommandLineOptions options,
                [Frozen, Substitute] IAmazonS3 s3,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                file.CreateZip(Any<string>()).Returns(zipFile);
                file.Sha256sum(Any<string>(), Any<CancellationToken>()).Returns(checksum);

                await service.PackageArtifact(command, cancellationToken);

                await s3.Received().PutObjectAsync(
                    Is<PutObjectRequest>(req =>
                        req.FilePath == zipFile &&
                        req.BucketName == options.S3BucketName &&
                        req.Key == checksum
                    ),
                    Is(cancellationToken)
                );
            }

            [Test, Auto]
            public async Task ShouldReturnLocationWithDownloadURL(
                string zipFile,
                string checksum,
                CommandMetadata command,
                [Frozen] CommandLineOptions options,
                [Frozen, Substitute] IAmazonS3 s3,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                file.CreateZip(Any<string>()).Returns(zipFile);
                file.Sha256sum(Any<string>(), Any<CancellationToken>()).Returns(checksum);

                var result = await service.PackageArtifact(command, cancellationToken);

                result.DownloadURL.Should().Be($"s3://{options.S3BucketName}/{checksum}");
            }

            [Test, Auto]
            public async Task ShouldReturnLocationWithChecksum(
                string zipFile,
                string checksum,
                CommandMetadata command,
                [Frozen] CommandLineOptions options,
                [Frozen, Substitute] IAmazonS3 s3,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                file.CreateZip(Any<string>()).Returns(zipFile);
                file.Sha256sum(Any<string>(), Any<CancellationToken>()).Returns(checksum);

                var result = await service.PackageArtifact(command, cancellationToken);

                result.Checksum.Should().Be(checksum);
            }

            [Test, Auto]
            public async Task ShouldReturnLocationWithAssemblyName(
                string zipFile,
                string checksum,
                CommandMetadata command,
                [Frozen] CommandLineOptions options,
                [Frozen, Substitute] IAmazonS3 s3,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                file.CreateZip(Any<string>()).Returns(zipFile);
                file.Sha256sum(Any<string>(), Any<CancellationToken>()).Returns(checksum);

                var result = await service.PackageArtifact(command, cancellationToken);

                result.AssemblyName.Should().Be(command.AssemblyName);
            }

            [Test, Auto]
            public async Task ShouldReturnLocationWithTypeName(
                string zipFile,
                string checksum,
                CommandMetadata command,
                [Frozen] CommandLineOptions options,
                [Frozen, Substitute] IAmazonS3 s3,
                [Frozen, Substitute] IFileService file,
                [Target] CommandService service,
                CancellationToken cancellationToken
            )
            {
                file.CreateZip(Any<string>()).Returns(zipFile);
                file.Sha256sum(Any<string>(), Any<CancellationToken>()).Returns(checksum);

                var result = await service.PackageArtifact(command, cancellationToken);

                result.TypeName.Should().Be(command.TypeName);
            }
        }
    }
}
