using System.Threading;
using System.Threading.Tasks;

namespace Brighid.Commands.Sdk.CompilationTests.EmbeddedCommandWithoutStartup
{
    [Command("test")]
    public class TestCommand : ICommand<TestCommandInput>
    {
        public async Task<CommandResult> Run(CommandContext<TestCommandInput> context, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new CommandResult("Hello World");
        }
    }
}
