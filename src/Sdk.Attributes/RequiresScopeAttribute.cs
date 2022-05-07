using System;
using System.Diagnostics;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Annotates a scope that a command requires.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [Conditional("CodeGeneration")]
    public class RequiresScopeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresScopeAttribute" /> class.
        /// </summary>
        /// <param name="scope">The scope that the command requires.</param>
        public RequiresScopeAttribute(string scope)
        {
            Scope = scope;
        }

        /// <summary>
        /// Gets the name of the required command scope.
        /// </summary>
        public string Scope { get; }
    }
}
