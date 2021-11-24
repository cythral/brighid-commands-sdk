using System;
using System.Diagnostics;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Attribute to indicate what type of startup class to use for a Command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the command.</param>
        public CommandAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the description of the command.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the role required to use this command.
        /// </summary>
        public string? RequiredRole { get; set; }

        /// <summary>
        /// Gets or sets the startup type of the command.
        /// </summary>
        public Type? StartupType { get; set; }
    }
}
