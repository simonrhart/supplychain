using System;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Framework.Command
{
    /// <summary>
    /// Interface for command.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface ICommand<in TContext> : IDisposable
    {
        /// <summary>
        /// Executes the context.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        Task ExecuteAsync(TContext context);

        bool IsInitialized { get; }
   
    }
}
