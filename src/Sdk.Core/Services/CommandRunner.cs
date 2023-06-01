using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Represents a wrapper around commands that takes raw command input, parses it and passes it to the command.
    /// </summary>
    /// <typeparam name="TInput">The type of input to use.</typeparam>
    public class CommandRunner<TInput> : ICommandRunner
    {
        private readonly ICommand<TInput> command;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRunner{TInput}" /> class.
        /// </summary>
        /// <param name="command">The command to run.</param>
        public CommandRunner(
            ICommand<TInput> command
        )
        {
            this.command = command;
        }

        /// <inheritdoc />
        public async Task<CommandResult> Run(CommandContext context, CancellationToken cancellationToken = default)
        {
            var input = await JsonSerializer.DeserializeAsync<TInput>(context.InputStream, cancellationToken: cancellationToken);
            var genericContext = new CommandContext<TInput>(input!, context.Principal, context.SourceSystem, context.SourceSystemChannel, context.SourceSystemUser, context.Token);
            return await command.Run(genericContext, cancellationToken);
        }
    }
}
