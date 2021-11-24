using System;
using System.Diagnostics;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Represents a command argument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    [Conditional("CodeGeneration")]
    public class ArgumentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentAttribute" /> class.
        /// </summary>
        /// <param name="index">The index of the argument.</param>
        public ArgumentAttribute(byte index)
        {
            Index = index;
        }

        /// <summary>
        /// Gets the argument index.
        /// </summary>
        public byte Index { get; }

        /// <summary>
        /// Gets or sets the argument description.
        /// </summary>
        public string? Description { get; set; }
    }
}
