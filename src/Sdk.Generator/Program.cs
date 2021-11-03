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
        }

        private static void SetupDependencyResolver(GeneratorExecutionContext context)
        {
            var additionalProbingPath = GetAdditionalProbingPath(context);
            var dllsInProbingPath = from file in Directory.GetFiles(additionalProbingPath, $"*.dll", SearchOption.AllDirectories)
                                    where file.Contains("netstandard") || file.Contains("net5.0")
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
