namespace Brighid.Commands.Sdk.CompilationTests.EmbeddedCommandWithoutStartup
{
    public class TestCommandInput
    {
        [Argument(0, Description = "ID to pass to the command.")]
        public string Id { get; set; } = string.Empty;
    }
}
