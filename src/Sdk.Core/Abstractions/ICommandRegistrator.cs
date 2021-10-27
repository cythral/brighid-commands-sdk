using Microsoft.Extensions.DependencyInjection;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Describes an object used for registering a command.
    /// </summary>
    public interface ICommandRegistrator
    {
        /// <summary>
        /// Registers a command and runner in a service collection and returns the resulting command runner.
        /// </summary>
        /// <param name="services">Service collection to inject the command and runner into.</param>
        /// <returns>The resulting command runner.</returns>
        ICommandRunner Register(IServiceCollection services);
    }
}
