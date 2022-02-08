namespace Brighid.Commands.Sdk.Models
{
    /// <summary>
    /// Represents a type of command parameter.
    /// </summary>
    public enum CommandParameterType : int
    {
        /// <summary>
        /// Parameter that is a sequence of any characters.
        /// </summary>
        String = 0,

        /// <summary>
        /// Parameter that is numeric.
        /// </summary>
        Number = 1,
    }
}
