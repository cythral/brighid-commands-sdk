using System.Text.Json.Serialization;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Represents a JSON Serializer context.
    /// </summary>
    [JsonSourceGenerationOptions]
    [JsonSerializable(typeof(CommandMetadata[]))]
    public partial class SerializerContext : JsonSerializerContext
    {
    }
}
