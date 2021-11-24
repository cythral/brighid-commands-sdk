namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Exception thrown when the dependency probing path was invalid or not found.
    /// </summary>
    public class MissingOutputPathException : GenerationFailureException
    {
        /// <inheritdoc />
        public override string Id => "BCS003";

        /// <inheritdoc />
        public override string Title => "Missing OutputPath MSBuild Property";

        /// <inheritdoc />
        public override string Description => "Could not find the Output Path in the project's MSBuild Properties.";
    }
}
