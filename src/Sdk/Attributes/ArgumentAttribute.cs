using System;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Represents a command argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentAttribute" /> class.
        /// </summary>
        /// <param name="index">The index of the argument.</param>
        public ArgumentAttribute(uint index)
        {
            Index = index;
        }

        /// <summary>
        /// Gets the argument index.
        /// </summary>
        public uint Index { get; }

        /// <summary>
        /// Gets or sets the argument description.
        /// </summary>
        public string? Description { get; set; }
    }
}
