using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Brighid.Commands.Sdk.PackageTool;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

#pragma warning disable SA1516 // Elements separated by blank line

SetupDependencyResolver();
Run(args);

static void SetupDependencyResolver()
{
    var additionalProbingPath = Environment.GetEnvironmentVariable("ADDITIONAL_PROBING_PATH")!;
    var dllsInProbingPath = from file in Directory.GetFiles(additionalProbingPath, $"*.dll", SearchOption.AllDirectories)
                            where file.Contains("netstandard") || file.Contains("net6.0") || file.Contains("net5.0")
                            select file;

    AssemblyLoadContext.Default.Resolving += (_, name) =>
    {
        var query = from dll in dllsInProbingPath
                    where dll.EndsWith($"{name.Name}.dll", true, null)
                    let assembly = TryLoadAssembly(dll)
                    where assembly != null
                    select assembly;

        return query.FirstOrDefault();
    };
}

static Assembly? TryLoadAssembly(string path)
{
    try
    {
        return Assembly.LoadFile(path);
    }
    catch (Exception)
    {
        return null;
    }
}

static void Run(string[] args)
{
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
}
