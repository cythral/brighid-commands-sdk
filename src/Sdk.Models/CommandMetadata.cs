using System.Collections.Generic;

namespace Brighid.Commands.Sdk.Models
{
    /// <summary>
    /// Metadata about a command.
    /// </summary>
    public struct CommandMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandMetadata" /> struct.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="typeName">The name of the command's type.</param>
        /// <param name="assemblyName">The name of the assembly containing the command.</param>
        /// <param name="intermediateOutputPath">The intermediate output path of the command's compilation.</param>
        /// <param name="outputPath">The output path of the command's compilation.</param>
        /// <param name="parameters">The parameters of the command.</param>
        /// <param name="scopes">The command's requested scopes.</param>
        public CommandMetadata(
            string name,
            string typeName,
            string assemblyName,
            string intermediateOutputPath,
            string outputPath,
            IEnumerable<CommandParameter> parameters,
            IEnumerable<string> scopes
        )
        {
            Name = name;
            TypeName = typeName;
            AssemblyName = assemblyName;
            IntermediateOutputPath = intermediateOutputPath;
            OutputPath = outputPath;
            Parameters = parameters;
            Scopes = scopes;
            Description = null;
            RequiredRole = null;
            StartupType = null;
        }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type name of the command's registrator.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the assembly the command is located in.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the intermediate output path of the command's compilation.
        /// </summary>
        public string IntermediateOutputPath { get; set; }

        /// <summary>
        /// Gets or sets the output path of the command's compilation.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// Gets or sets the command's parameters.
        /// </summary>
        public IEnumerable<CommandParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the command's scopes.
        /// </summary>
        public IEnumerable<string> Scopes { get; set; }

        /// <summary>
        /// Gets or sets the command's description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the role required to use the command.
        /// </summary>
        public string? RequiredRole { get; set; }

        /// <summary>
        /// Gets or sets the startup type of the command.
        /// </summary>
        public string? StartupType { get; set; }
    }
}
