using System.Runtime.Serialization;

using YamlDotNet.Serialization;

namespace Brighid.Commands.Sdk.Models
{
    /// <summary>
    /// Represents a parameter of a command.
    /// </summary>
    [DataContract]
    public struct CommandParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandParameter" /> struct.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="description">The description of the parameter.</param>
        /// <param name="type">The type of the parameter.</param>
        /// <param name="argumentIndex">The argument index.</param>
        public CommandParameter(
            string name,
            string? description = null,
            CommandParameterType type = default,
            byte? argumentIndex = null
        )
        {
            Name = name;
            Description = description;
            Type = type;
            ArgumentIndex = argumentIndex;
        }

        /// <summary>
        /// Gets or sets the name of the command parameter.
        /// </summary>
        [DataMember(Name = "n")]
        [YamlMember(Alias = "n")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the command parameter.
        /// </summary>
        [DataMember(Name = "d")]
        [YamlMember(Alias = "d")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the command parameter.
        /// </summary>
        [DataMember(Name = "t")]
        [YamlMember(Alias = "t")]
        public CommandParameterType Type { get; set; }

        /// <summary>
        /// Gets or sets the argument index, if this parameter can be used as an argument.
        /// </summary>
        [DataMember(Name = "i")]
        [YamlMember(Alias = "i")]
        public byte? ArgumentIndex { get; set; }
    }
}
