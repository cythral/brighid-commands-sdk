using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;

namespace Brighid.Commands.Sdk.Generator
{
    /// <inheritdoc />
    public class TypeUtils : ITypeUtils
    {
        /// <inheritdoc />
        public bool IsSymbolEqualToType(INamedTypeSymbol symbol, Type type)
        {
            if (symbol == null)
            {
                return false;
            }

            var assembly = symbol.ContainingAssembly;
            var typeAssemblyInfo = type.Assembly.GetName();
            var format = new SymbolDisplayFormat(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
            var name = symbol.ToDisplayString(format);
            var assemblyName = assembly?.MetadataName;
            var version = assembly?.Identity.Version;
            var publicKeyToken = assembly?.Identity.PublicKeyToken;

#pragma warning disable IDE0078
            if (assembly?.MetadataName == "System.Runtime" || assembly?.MetadataName == "System.Private.CoreLib")
            {
                var runtimeAssemblyInfo = typeof(object).Assembly.GetName();
                assemblyName = runtimeAssemblyInfo.Name;
                version = runtimeAssemblyInfo.Version;
                publicKeyToken = runtimeAssemblyInfo.GetPublicKeyToken()?.ToImmutableArray();
            }
#pragma warning restore IDE0078

            return
                name == type.FullName &&
                assemblyName == typeAssemblyInfo.Name &&
                version == typeAssemblyInfo.Version &&
                (publicKeyToken?.SequenceEqual(typeAssemblyInfo.GetPublicKeyToken()!) ?? typeAssemblyInfo == null);
        }

        /// <inheritdoc />
        public TAttribute LoadAttributeData<TAttribute>(AttributeData attributeData)
            where TAttribute : Attribute
        {
            var parameters = from arg in attributeData.ConstructorArguments
                             select arg.Value;

            var result = (TAttribute?)Activator.CreateInstance(typeof(TAttribute), parameters.ToArray());
            foreach (var (key, constant) in attributeData.NamedArguments)
            {
                var value = constant.Value;
                if (value is INamedTypeSymbol)
                {
                    value = null;
                }

                var prop = typeof(TAttribute).GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                var setter = prop?.GetSetMethod(true);
                setter?.Invoke(result, new[] { value });
            }

            return result ?? throw new CannotLoadAttributeException(typeof(TAttribute));
        }

        /// <inheritdoc />
        public INamedTypeSymbol? GetNamedTypeArgument(AttributeData attributeData, string argumentName)
        {
            var query = from arg in attributeData.NamedArguments
                        where arg.Key == argumentName && arg.Value.Value is INamedTypeSymbol
                        select (INamedTypeSymbol)arg.Value.Value!;

            return query.FirstOrDefault();
        }
    }
}
