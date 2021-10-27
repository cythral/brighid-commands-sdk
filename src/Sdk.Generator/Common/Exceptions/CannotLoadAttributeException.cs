using System;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Exception thrown when the dependency probing path was invalid or not found.
    /// </summary>
    public class CannotLoadAttributeException : GenerationFailureException
    {
        private readonly Type attributeType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CannotLoadAttributeException" /> class.
        /// </summary>
        /// <param name="attributeType">The type of attribute that was attempted to instantiate.</param>
        public CannotLoadAttributeException(
            Type attributeType
        )
        {
            this.attributeType = attributeType;
        }

        /// <inheritdoc />
        public override string Id => "BCS001";

        /// <inheritdoc />
        public override string Title => "Cannot Load Attribute from Attribute Data";

        /// <inheritdoc />
        public override string Description => $"Attempted to instantiate an attribute of type {attributeType.FullName} from attribute data but failed.";
    }
}
