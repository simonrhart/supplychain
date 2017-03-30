namespace Microsoft.SupplyChain.Framework
{
    /// <summary>
    /// Interface for command.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface ICommand<in TContext>
    {
        /// <summary>
        /// Executes the context.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        void Execute(TContext context);
   
    }
}
