using System;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Utilities for dealing with types and symbols related to types.
    /// </summary>
    public interface ITypeUtils
    {
        /// <summary>
        /// Determines whether a symbol represents/is equal to a type.
        /// </summary>
        /// <param name="symbol">The symbol to compare.</param>
        /// <param name="type">The type to compare.</param>
        /// <returns>True if the symbol represents the type, or false if not.</returns>
        bool IsSymbolEqualToType(INamedTypeSymbol symbol, Type type);

        /// <summary>
        /// Creates a concrete attribute from attribute data.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute to create.</typeparam>
        /// <param name="attributeData">The attribute's data source.</param>
        /// <returns>The resulting attribute.</returns>
        TAttribute LoadAttributeData<TAttribute>(AttributeData attributeData)
            where TAttribute : Attribute;

        /// <summary>
        /// Gets a named type argument from an attribute.
        /// </summary>
        /// <param name="attributeData">The attribute to get a named type argument for.</param>
        /// <param name="argumentName">The name of the argument to get a value for.</param>
        /// <returns>The value of the argument if it exists, or null if not.</returns>
        INamedTypeSymbol? GetNamedTypeArgument(AttributeData attributeData, string argumentName);
    }
}
