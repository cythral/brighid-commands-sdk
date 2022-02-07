using System.Collections.Generic;

using Brighid.Commands.Sdk.PackageTool;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

#pragma warning disable SA1516 // Elements separated by blank line

Host.CreateDefaultBuilder()
.UseConsoleLifetime()
.UseSerilog(dispose: true)
.ConfigureLogging(options =>
{
    options.AddSimpleConsole(consoleOptions =>
    {
        consoleOptions.SingleLine = true;
    });
})
.ConfigureHostConfiguration(options =>
{
    if (args.Length > 1)
    {
        options.AddCommandLine(args[1..], new Dictionary<string, string>
        {
            ["--bucket-name"] = "CommandLine:S3BucketName",
            ["--destination"] = "CommandLine:TemplateDestination",
        });
    }
})
.ConfigureServices((context, services) =>
{
    new Startup(context.Configuration).ConfigureServices(services);
})
.Build()
.Run();
