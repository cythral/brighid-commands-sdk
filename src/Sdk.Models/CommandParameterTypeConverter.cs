using System;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Brighid.Commands.Sdk.Models
{
    /// <summary>
    /// Converter that converts command parameter types into yaml values and vice versa.
    /// </summary>
    public class CommandParameterTypeConverter : IYamlTypeConverter
    {
        /// <inheritdoc />
        public bool Accepts(Type type)
        {
            return type == typeof(CommandParameterType);
        }

        /// <inheritdoc />
        public object? ReadYaml(IParser parser, Type type)
        {
            var value = int.Parse(parser.Consume<Scalar>().Value);
            return Enum.ToObject(typeof(CommandParameterType), value);
        }

        /// <inheritdoc />
        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var enumValue = (int?)(CommandParameterType?)value;
            var scalar = new Scalar(enumValue.ToString()!);
            emitter.Emit(scalar);
        }
    }
}
