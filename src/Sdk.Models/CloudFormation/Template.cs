using System.Collections.Generic;

namespace Brighid.Commands.Sdk.Models.CloudFormation
{
    /// <summary>
    /// Represents a CloudFormation Template.
    /// </summary>
    public class Template
    {
        /// <summary>
        /// Gets or sets the template's resources.
        /// </summary>
        public Dictionary<string, CommandResource> Resources { get; set; } = new();
    }
}
