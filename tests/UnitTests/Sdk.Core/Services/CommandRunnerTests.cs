using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;

using FluentAssertions;

using NSubstitute;

using NUnit.Framework;

using static NSubstitute.Arg;

namespace Brighid.Commands.Sdk
{
    public class CommandRunnerTests
    {
        [TestFixture]
        public class RunTests
        {
            [Test, Auto]
            public async Task ShouldParseInputAndPassItToTheCommand(
                string id,
                string sourceSystem,
                string sourceSystemId,
                string token,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemId, token);

                await runner.Run(context, cancellationToken);

                await command.Received().Run(Is<CommandContext<TestCommandInput>>(context => context.Input.Id == id), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldPassPrincipalToTheCommand(
                string id,
                string sourceSystem,
                string sourceSystemId,
                string token,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemId, token);

                await runner.Run(context, cancellationToken);

                await command.Received().Run(Is<CommandContext<TestCommandInput>>(context => context.Principal == principal), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldPassSourceSystemToTheCommand(
                string id,
                string sourceSystem,
                string sourceSystemId,
                string token,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemId, token);

                await runner.Run(context, cancellationToken);

                await command.Received().Run(Is<CommandContext<TestCommandInput>>(context => context.SourceSystem == sourceSystem), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldPassSourceSystemChannelToTheCommand(
                string id,
                string sourceSystem,
                string sourceSystemChannel,
                string sourceSystemUser,
                string token,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemChannel, sourceSystemUser, token);

                await runner.Run(context, cancellationToken);

                await command.Received().Run(Is<CommandContext<TestCommandInput>>(context => context.SourceSystemChannel == sourceSystemChannel), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldPassSourceSystemUserToTheCommand(
                string id,
                string sourceSystem,
                string sourceSystemChannel,
                string sourceSystemUser,
                string token,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemChannel, sourceSystemUser, token);

                await runner.Run(context, cancellationToken);

                await command.Received().Run(Is<CommandContext<TestCommandInput>>(context => context.SourceSystemUser == sourceSystemUser), Is(cancellationToken));
            }

            [Test, Auto]
            public async Task ShouldReturnTheCommandOutput(
                string id,
                string sourceSystem,
                string sourceSystemId,
                string token,
                CommandResult expectedResult,
                ClaimsPrincipal principal,
                [Frozen, Substitute] ICommand<TestCommandInput> command,
                [Target] CommandRunner<TestCommandInput> runner,
                CancellationToken cancellationToken
            )
            {
                command.Run(Any<CommandContext<TestCommandInput>>(), Any<CancellationToken>()).Returns(expectedResult);
                var input = Encoding.UTF8.GetBytes($"{{\"Id\":\"{id}\"}}");
                var stream = new MemoryStream(input);
                var context = new CommandContext(stream, principal, sourceSystem, sourceSystemId, token);

                var result = await runner.Run(context, cancellationToken);

                result.Should().Be(expectedResult);
            }
        }
    }
}
