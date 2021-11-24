using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Host service for the package tool.
    /// </summary>
    public class PackageToolHost : IHost
    {
        private readonly IHostApplicationLifetime lifetime;

        private readonly IFileService file;

        private readonly IEnvironmentService environment;

        private readonly CommandLineOptions options;

        private readonly ILogger<PackageToolHost> logger;

        private readonly ITemplateService templateService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageToolHost" /> class.
        /// </summary>
        /// <param name="lifetime">Service for controlling the application lifetime.</param>
        /// <param name="environment">Service for interacting with the environment.</param>
        /// <param name="file">Service for interacting with files.</param>
        /// <param name="templateService">Service for dealing with templates.</param>
        /// <param name="options">Options specified on the command-line.</param>
        /// <param name="logger">Service used for logging information to some destination(s).</param>
        /// <param name="services">The services to use for the package tool host.</param>
        public PackageToolHost(
            IHostApplicationLifetime lifetime,
            IFileService file,
            IEnvironmentService environment,
            ITemplateService templateService,
            IOptions<CommandLineOptions> options,
            ILogger<PackageToolHost> logger,
            IServiceProvider services
        )
        {
            Services = services;
            this.environment = environment;
            this.templateService = templateService;
            this.options = options.Value;
            this.file = file;
            this.logger = logger;
            this.lifetime = lifetime;
        }

        /// <inheritdoc />
        public IServiceProvider Services { get; }

        /// <inheritdoc />
        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.CompletedTask;

            try
            {
                var args = environment.GetCommandLineArguments();
                if (args.Length < 2 || !file.Exists(args[1]))
                {
                    throw new Exception("Argument 1 must be a valid file.");
                }

                var temporaryTemplatePath = await GenerateTemplate(args[1], cancellationToken);
                file.Move(temporaryTemplatePath, options.TemplateDestination);
            }
            catch (Exception e)
            {
                logger.LogCritical("{@message}", e.Message);
                environment.ExitCode = 1;
            }

            lifetime.StopApplication();
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            Console.Out.Flush();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task<string> GenerateTemplate(string metadataFile, CancellationToken cancellationToken)
        {
            logger.LogInformation("Reading file: {@file}", metadataFile);
            var metadata = await file.ReadCommandMetadata(metadataFile, cancellationToken);
            var temporaryFilePath = file.GenerateTemporaryFilePath();
            using var stream = file.OpenWrite(temporaryFilePath);
            await templateService.GenerateTemplate(stream, metadata, cancellationToken);
            return temporaryFilePath;
        }
    }
}
