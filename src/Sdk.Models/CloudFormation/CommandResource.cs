#pragma warning disable CA1822

namespace Brighid.Commands.Sdk.Models.CloudFormation
{
    /// <summary>
    /// Represents a resource within a CloudFormation template.
    /// </summary>
    public class CommandResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResource" /> class.
        /// </summary>
        public CommandResource()
        {
            Properties = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResource" /> class.
        /// </summary>
        /// <param name="properties">The properties to pass to the resource.</param>
        public CommandResource(CommandResourceProperties properties)
        {
            Properties = properties;
        }

        /// <summary>
        /// Gets or sets the resource type.
        /// </summary>
        public string Type { get; set; } = "Custom::Brighid::Command";

        /// <summary>
        /// Gets or sets the resource's properties.
        /// </summary>
        public CommandResourceProperties Properties { get; set; }
    }
}
