using System.IO;
using System.Security.Claims;

namespace Brighid.Commands.Sdk
{
    /// <summary>
    /// Context holder for executing commands.
    /// </summary>
    /// <typeparam name="TInput">The type of input to use.</typeparam>
    public class CommandContext<TInput> : CommandContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext{TInput}" /> class.
        /// </summary>
        /// <param name="input">The input parameter to use when invoking the command.</param>
        /// <param name="principal">The principal invoking the command.</param>
        /// <param name="sourceSystem">The name of the system the command is being invoked from.</param>
        /// <param name="sourceSystemId">ID of the sender/channel in the source system the command request originated from.</param>
        public CommandContext(
            TInput input,
            ClaimsPrincipal principal,
            string sourceSystem,
            string sourceSystemId
        )
        : base(
            MemoryStream.Null,
            principal,
            sourceSystem,
            sourceSystemId
        )
        {
            Input = input;
        }

        /// <summary>
        /// Gets the input stream containing command arguments and options.
        /// </summary>
        public TInput Input { get; }
    }
}
