namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Exception thrown when the dependency probing path was invalid or not found.
    /// </summary>
    public class InvalidOrMissingProbingPathException : GenerationFailureException
    {
        /// <inheritdoc />
        public override string Id => "BCS000";

        /// <inheritdoc />
        public override string Title => "Invalid or Missing Probing Path";

        /// <inheritdoc />
        public override string Description => "Dependencies cannot be loaded because the BrighidCommandsAdditionalProbingPath property was not set or is invalid.";
    }
}
