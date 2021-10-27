using System;
using System.IO;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

namespace Brighid.Commands.Sdk.CompilationTests
{
    [TestFixture]
    [Category("Integration")]
    public class EmbeddedCommandWithoutStartupTests
    {
        private const string ProjectPath = "CompilationTests/Projects/EmbeddedCommandWithoutStartup/EmbeddedCommandWithoutStartup.csproj";
        private static Project project = null!;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Directory.SetCurrentDirectory(TestMetadata.TestDirectory);
            project = await MSBuildProjectExtensions.LoadProject(ProjectPath);
        }

        [Test, Auto]
        public async Task RegisterShouldReturnCommandRunner()
        {
            var services = new ServiceCollection();
            using var generation = await project.GenerateAssembly();
            var (assembly, _) = generation;
            var registratorType = assembly.GetType("Brighid.Commands.Sdk.CompilationTests.EmbeddedCommandWithoutStartup.TestCommandRegistrator");
            var registerMethod = registratorType!.GetMethod("Register");
            var registrator = (ICommandRegistrator)Activator.CreateInstance(registratorType!, Array.Empty<object>())!;

            var runner = registrator.Register(services);
            runner.Should().NotBeNull();
        }
    }
}
