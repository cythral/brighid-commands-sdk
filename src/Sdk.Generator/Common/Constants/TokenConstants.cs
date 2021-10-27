using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Brighid.Commands.Sdk.Generator
{
    /// <summary>
    /// Syntax token constants.
    /// </summary>
    public static class TokenConstants
    {
        /// <summary>
        /// The colon token.
        /// </summary>
        public static readonly SyntaxToken ColonToken = Token(SyntaxKind.ColonToken);

        /// <summary>
        /// The partial modifier.
        /// </summary>
        public static readonly SyntaxToken PartialModifier = Token(SyntaxKind.PartialKeyword);

        /// <summary>
        /// The public accessibility modifier.
        /// </summary>
        public static readonly SyntaxToken PublicModifier = Token(SyntaxKind.PublicKeyword);

        /// <summary>
        /// The private accessibility modifier.
        /// </summary>
        public static readonly SyntaxToken PrivateModifier = Token(SyntaxKind.PrivateKeyword);

        /// <summary>
        /// The readonly accessibility modifier.
        /// </summary>
        public static readonly SyntaxToken ReadOnlyModifier = Token(SyntaxKind.ReadOnlyKeyword);

        /// <summary>
        /// The async modifier.
        /// </summary>
        public static readonly SyntaxToken AsyncModifier = Token(SyntaxKind.AsyncKeyword);

        /// <summary>
        /// The end of file token.
        /// </summary>
        public static readonly SyntaxToken EndOfFile = Token(SyntaxKind.EndOfFileToken);

        /// <summary>
        /// The disable keyword.
        /// </summary>
        public static readonly SyntaxToken DisableKeyword = Token(SyntaxKind.DisableKeyword);
    }
}
