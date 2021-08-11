using System;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Annotates a command option.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the option description.
        /// </summary>
        public string? Description { get; set; }
    }
}
