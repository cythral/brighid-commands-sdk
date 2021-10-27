using Brighid.Commands.Sdk.Generator.CommandRegistrators;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Builder that creates a generator host and runs it.
    /// </summary>
    public class Startup
    {
        private readonly GeneratorContext context;
        private readonly ClassSyntaxReceiver receiver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="context">The generator context.</param>
        /// <param name="receiver">The receiver that visited class nodes.</param>
        public Startup(
            GeneratorContext context,
            ClassSyntaxReceiver receiver
        )
        {
            this.context = context;
            this.receiver = receiver;
        }

        /// <summary>
        /// Builds the generator host and runs it.
        /// </summary>
        public void Run()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Build()
                .RunAsync(context.ExecutionContext.CancellationToken)
                .GetAwaiter()
                .GetResult();
        }

        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(this.context);
            services.AddScoped<CommandContext>();
            services.AddSingleton<IHost, SourceGeneratorHost>();
            services.AddSingleton<ITypeUtils, TypeUtils>();
            services.AddSingleton<ISyntaxUtils, SyntaxUtils>();
            services.AddSingleton<ICommandRegistratorGenerator, CommandRegistratorGenerator>();
            services.AddSingleton(receiver);
            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddConsole();
            });
        }
    }
}
