using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Represents the context of a source generator pass.
    /// </summary>
    public class GeneratorContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorContext" /> class.
        /// </summary>
        /// <param name="executionContext">The execution context of the generator.</param>
        public GeneratorContext(
            GeneratorExecutionContext executionContext
        )
        {
            ExecutionContext = executionContext;
        }

        /// <summary>
        /// Gets the execution context.
        /// </summary>
        public GeneratorExecutionContext ExecutionContext { get; }
    }
}
