using System;
using System.Collections.Generic;

namespace Brighid.Commands.Sdk.Models
{
    /// <summary>
    /// Represents a command that can be executed.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Gets or sets the type of command this is.
        /// </summary>
        public CommandType Type { get; set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role users need to execute this command.
        /// </summary>
        public string? RequiredRole { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the location of an embedded command.
        /// </summary>
        public EmbeddedCommandLocation? EmbeddedLocation { get; set; }

        /// <summary>
        /// Gets or sets the command's parameters.
        /// </summary>
        public IEnumerable<CommandParameter> Parameters { get; set; } = Array.Empty<CommandParameter>();

        /// <summary>
        /// Gets or sets the command's scopes.
        /// </summary>
        public IEnumerable<string> Scopes { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets a value indicating whether or not this command is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
