using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Common type syntax constants.
    /// </summary>
    public static class TypeConstants
    {
        /// <summary>
        /// Gets the Command Registrator Interface Type Syntax.
        /// </summary>
        public static readonly TypeSyntax CommandRegistratorInterfaceType = ParseTypeName("Brighid.Commands.Sdk.ICommandRegistrator");

        /// <summary>
        /// Gets the CommandContext Type Syntax.
        /// </summary>
        public static readonly TypeSyntax CommandContextType = ParseTypeName("Brighid.Commands.Sdk.CommandContext");

        /// <summary>
        /// Gets the CancellationToken Type Syntax.
        /// </summary>
        public static readonly TypeSyntax CancellationTokenType = ParseTypeName("System.Threading.CancellationToken");
    }
}
