using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Brighid.Commands.Sdk.Models;

namespace Brighid.Commands.Sdk.PackageTool
{
    /// <inheritdoc />
    public class FileService : IFileService
    {
        /// <inheritdoc />
        public bool Exists(string file)
        {
            return File.Exists(file);
        }

        /// <inheritdoc />
        public Stream OpenWrite(string file)
        {
            return File.OpenWrite(file);
        }

        /// <inheritdoc />
        public void Move(string source, string destination)
        {
            File.Move(source, destination, true);
        }

        /// <inheritdoc />
        public string CreateZip(string directory)
        {
            var zipFileName = GenerateTemporaryFilePath();
            ZipFile.CreateFromDirectory(directory, zipFileName);

            using var zipArchive = ZipFile.Open(zipFileName, ZipArchiveMode.Update);
            foreach (var entry in zipArchive.Entries)
            {
                entry.ExternalAttributes |= Convert.ToInt32("755", 8) << 16;
            }

            return zipFileName;
        }

        /// <inheritdoc />
        public async Task<string> Sha256sum(string file, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var hasher = SHA256.Create();
            using var fileStream = File.OpenRead(file);
            var bytes = await hasher.ComputeHashAsync(fileStream, cancellationToken);
            var sum = Convert.ToBase64String(bytes);

            return sum
                .Replace('+', '-')
                .Replace('/', '_');
        }

        /// <inheritdoc />
        public async Task<CommandMetadata[]> ReadCommandMetadata(string file, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var stream = File.OpenRead(file);
            return await JsonSerializer.DeserializeAsync(stream, SerializerContext.Default.CommandMetadataArray, cancellationToken) ?? throw new Exception("Could not deserialize template metadata.");
        }

        /// <inheritdoc />
        public string GenerateTemporaryFilePath()
        {
            return Path.GetTempFileName() + Path.GetRandomFileName();
        }
    }
}
