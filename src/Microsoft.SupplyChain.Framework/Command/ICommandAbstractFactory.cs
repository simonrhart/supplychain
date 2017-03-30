namespace Microsoft.SupplyChain.Framework
{
    /// <summary>
    /// Interface that allows for injection for executing commands.
    /// </summary>
    public interface ICommandAbstractFactory
    {
        /// <summary>
        /// Executes the passed context.
        /// </summary>
        /// <typeparam name="TContext">The type of context..</typeparam>
        /// <param name="context">The context to execute.</param>
        void ExecuteCommand<TContext>(TContext context);
    }
}
