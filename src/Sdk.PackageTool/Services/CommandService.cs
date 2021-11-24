using System.Threading;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;

using Brighid.Commands.Sdk.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <inheritdoc />
    public class CommandService : ICommandService
    {
        private readonly IAmazonS3 s3;

        private readonly IFileService file;

        private readonly ILogger<CommandService> logger;

        private readonly CommandLineOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandService" /> class.
        /// </summary>
        /// <param name="s3">Amazon S3 Client.</param>
        /// <param name="file">Service for interacting with files.</param>
        /// <param name="logger">Logger used for logging info to some destination(s).</param>
        /// <param name="options">Options specified on the command-line.</param>
        public CommandService(
            IAmazonS3 s3,
            IFileService file,
            ILogger<CommandService> logger,
            IOptions<CommandLineOptions> options
        )
        {
            this.s3 = s3;
            this.file = file;
            this.logger = logger;
            this.options = options.Value;
        }

        /// <inheritdoc />
        public async Task<EmbeddedCommandLocation> PackageArtifact(CommandMetadata command, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var zipFile = file.CreateZip(command.OutputPath);
            logger.LogInformation("Zipped directory and saved file to: {@zipFile}", zipFile);

            var checksum = await file.Sha256sum(zipFile, cancellationToken);
            logger.LogInformation("Computed sha256sum of zip file: {@checksum}", checksum);

            var putObjectRequest = new PutObjectRequest
            {
                FilePath = zipFile,
                BucketName = options.S3BucketName,
                Key = checksum,
            };

            var putObjectResponse = await s3.PutObjectAsync(putObjectRequest, cancellationToken);
            logger.LogInformation("Received s3:PutObject response: {@putObjectResponse}", putObjectResponse);

            return new EmbeddedCommandLocation
            {
                DownloadURL = $"s3://{options.S3BucketName}/{checksum}",
                Checksum = checksum,
                AssemblyName = command.AssemblyName,
                TypeName = command.TypeName,
            };
        }
    }
}
