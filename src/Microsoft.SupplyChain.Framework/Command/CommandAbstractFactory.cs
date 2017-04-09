using System;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Framework.Command
{
    /// <summary>
    /// Abstract factory for executing commands.
    /// </summary>
    public class CommandAbstractFactory : ICommandAbstractFactory
    {
        /// <summary>
        /// Our service locator to work with.
        /// </summary>
        private readonly IServiceLocator _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAbstractFactory"/> class.
        /// </summary>
        /// <param name="resolver">The dependency resolver.</param>
        public CommandAbstractFactory(IServiceLocator resolver)
        {
            _resolver = resolver;
        }

        /// <summary>
        /// Looks up the command from the context and executes it.
        /// </summary>
        /// <typeparam name="TContext">The type of context to execute.</typeparam>
        /// <param name="context">The context to execute.</param>
        public async Task ExecuteCommandAsync<TContext>(TContext context)
        {
            var command = _resolver.GetInstance<ICommand<TContext>>();

            await command.ExecuteAsync(context);
        
            var disposable = command as IDisposable;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
    }
}
