using Microsoft.Extensions.DependencyInjection;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Factory for creating command runners.
    /// </summary>
    public class EmbeddedCommandRunnerFactory
    {
        /// <summary>
        /// Creates a new command runner.
        /// </summary>
        /// <typeparam name="TCommand">The command to create a runner for.</typeparam>
        /// <typeparam name="TCommandInput">The type of input to pass to the command.</typeparam>
        /// <typeparam name="TCommandStartup">The type of command startup to use.</typeparam>
        /// <param name="services">Service collection to use for the command.</param>
        /// <returns>The resulting command runner.</returns>
        public static ICommandRunner Create<TCommand, TCommandInput, TCommandStartup>(
            IServiceCollection services
        )
            where TCommand : class, ICommand<TCommandInput>
            where TCommandStartup : class, ICommandStartup
        {
            var startup = services
            .AddSingleton<ICommandStartup, TCommandStartup>()
            .BuildServiceProvider()
            .GetRequiredService<ICommandStartup>();

            startup.ConfigureServices(services);

            return Create<TCommand, TCommandInput>(services);
        }

        /// <summary>
        /// Creates a command runner without the use of a startup class.
        /// </summary>
        /// <typeparam name="TCommand">The command to create a runner for.</typeparam>
        /// <typeparam name="TCommandInput">The type of input to pass to the command.</typeparam>
        /// <param name="services">Service collection to use for the command.</param>
        /// <returns>The resulting command runner.</returns>
        public static ICommandRunner Create<TCommand, TCommandInput>(
            IServiceCollection services
        )
            where TCommand : class, ICommand<TCommandInput>
        {
            return services
                .AddSingleton<ICommand<TCommandInput>, TCommand>()
                .AddSingleton<ICommandRunner, CommandRunner<TCommandInput>>()
                .BuildServiceProvider()
                .GetRequiredService<ICommandRunner>();
        }
    }
}
