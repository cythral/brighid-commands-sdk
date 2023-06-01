using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Generates code for Brighid Commands.
    /// </summary>
    [Generator]
    public class Program : ISourceGenerator
    {
        private readonly ClassSyntaxReceiver receiver = new();

        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => receiver);
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                SetupDependencyResolver(context);
                RunGenerator(context);
            }
            catch (GenerationFailureException exception)
            {
                context.ReportDiagnostic(exception);
            }
            catch (Exception exception)
            {
                context.ReportDiagnostic(new GenerationFailureException { Description = exception.Message });
            }
        }

        private static void SetupDependencyResolver(GeneratorExecutionContext context)
        {
            var additionalProbingPath = GetAdditionalProbingPath(context);

            AssemblyLoadContext.Default.Resolving += (_, name) =>
            {
                var matchingFiles = from file in Directory.GetFiles(additionalProbingPath, $"{name.Name}.dll", SearchOption.AllDirectories)
                                    where file.Contains("netstandard") || file.Contains("net5.0") || file.Contains("net6.0") || file.Contains("net7.0")
                                    select file;

                foreach (var matchingFile in matchingFiles)
                {
                    try
                    {
                        var assembly = Assembly.LoadFile(matchingFile);
                        if (assembly.GetName().Version >= name.Version)
                        {
                            return assembly;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                return null;
            };
        }

        private static Assembly? TryLoadAssembly(string path)
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

        private static string GetAdditionalProbingPath(GeneratorExecutionContext context)
        {
            var options = context.AnalyzerConfigOptions.GlobalOptions;
            options.TryGetValue("build_property.BrighidCommandsAdditionalProbingPath", out var additionalProbingPath);

            return string.IsNullOrEmpty(additionalProbingPath)
                ? throw new InvalidOrMissingProbingPathException()
                : additionalProbingPath;
        }

        private void RunGenerator(GeneratorExecutionContext context)
        {
            var generatorContext = new GeneratorContext(context);
            var startup = new Startup(generatorContext, receiver);
            startup.Run();
        }
    }
}
