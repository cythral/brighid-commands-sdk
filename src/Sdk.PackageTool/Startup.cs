using Amazon.S3;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Performs startup configuration for the package tool's host.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext:s}] {Message:lj} {Properties:j} {Exception}{NewLine}")
                .CreateLogger();
        }

        /// <summary>
        /// Adds services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CommandLineOptions>(configuration.GetSection("CommandLine").Bind);
            services.AddSingleton<IHost, PackageToolHost>();
            services.AddSingleton<IEnvironmentService, EnvironmentService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ITemplateService, TemplateService>();
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<IAmazonS3, AmazonS3Client>();
        }
    }
}
