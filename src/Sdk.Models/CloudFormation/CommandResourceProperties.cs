#pragma warning disable CA1822

namespace Brighid.Commands.Sdk.Models.CloudFormation
{
    /// <inheritdoc />
    public class CommandResourceProperties : Command
    {
        /// <summary>
        /// Gets or sets the service token of the command resource.
        /// </summary>
        public string ServiceToken { get; set; } = "{{resolve:ssm:/brighid/commands/resources/command/resource-arn}}";
    }
}
