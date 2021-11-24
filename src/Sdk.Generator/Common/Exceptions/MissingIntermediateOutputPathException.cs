namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Exception thrown when the dependency probing path was invalid or not found.
    /// </summary>
    public class MissingIntermediateOutputPathException : GenerationFailureException
    {
        /// <inheritdoc />
        public override string Id => "BCS002";

        /// <inheritdoc />
        public override string Title => "Missing IntermediateOutputPath MSBuild Property";

        /// <inheritdoc />
        public override string Description => "Could not find the Intermediate Output Path in the project's MSBuild Properties.";
    }
}
