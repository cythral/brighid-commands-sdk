namespace Brighid.Commands.Sdk.PackageTool
{
    /// <summary>
    /// Options specified as arguments on the command-line.
    /// </summary>
    public class CommandLineOptions
    {
        /// <summary>
        /// Gets or sets the S3 bucket name where artifacts are uploaded to.
        /// </summary>
        public string S3BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location to write the template to.
        /// </summary>
        public string TemplateDestination { get; set; } = string.Empty;
    }
}
