using System;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Exception thrown when the generator fails to generate source code.
    /// </summary>
    public class GenerationFailureException : Exception
    {
        /// <summary>
        /// Gets the error ID.
        /// </summary>
        public virtual string Id { get; init; } = string.Empty;

        /// <summary>
        /// Gets the error title.
        /// </summary>
        public virtual string Title { get; init; } = string.Empty;

        /// <summary>
        /// Gets the error description.
        /// </summary>
        public virtual string Description { get; init; } = string.Empty;

        /// <summary>
        /// Gets the category of the error.
        /// </summary>
        public virtual string Category { get; init; } = "BrighidCommands";

        /// <summary>
        /// Gets the location of the error.
        /// </summary>
        public virtual Location Location { get; init; } = Location.None;

        /// <summary>
        /// Casts a generation failure exception to a diagnostic.
        /// </summary>
        /// <param name="exception">The exception to cast.</param>
        public static implicit operator Diagnostic(GenerationFailureException exception)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(exception.Id, exception.Title, exception.Description, exception.Category, DiagnosticSeverity.Error, true), exception.Location);
        }
    }
}
