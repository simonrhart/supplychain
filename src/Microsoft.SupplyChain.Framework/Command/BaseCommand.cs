using System;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Framework.Command
{
    /// <summary>
    /// Base command 
    /// </summary>
    /// <typeparam name="TContext">The context for this command.</typeparam>
    public abstract class BaseCommand<TContext> : ICommand<TContext> where TContext : BaseContext
    {
        private bool _initialized;
        private bool _disposed;
        private TContext _context;

        /// <summary>
        /// Used to control how to handle exceptions.
        /// </summary>
        protected enum ExceptionAction
        {
            /// <summary>
            /// Exception has been handled.
            /// </summary>
            Suppress,

            /// <summary>
            /// Throw the exception.
            /// </summary>
            Rethrow
        }

        public enum TearDownAction
        {
            OnDispose,
            OnExecuteExit
        }

        /// <summary>
        /// Executes the passed context.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        public async Task ExecuteAsync(TContext context)
        {
            _context = context;

            try
            {
                Initialize(context);
                await DoExecuteAsync(context);
            }
            catch (Exception e)
            {
                if (HandleError(context, e) == ExceptionAction.Rethrow)
                {
                    throw;
                }
            }
            finally
            {
                if (this.HandleTearDown() == TearDownAction.OnExecuteExit)
                    TearDown(context);
            }
           
        }

        public bool IsInitialized => _initialized;

        internal void Initialize(TContext context)
        {
            try
            {
                //only initialize on startup.
                if (!_initialized)
                {
                    DoInitialize(context);
                    _initialized = true;
                }
            }
            catch (Exception e)
            {
                if (HandleError(context, e) == ExceptionAction.Rethrow)
                {
                    throw;
                }
            }
        }

        internal void TearDown(TContext context)
        {
            try
            {
                DoTearDown(context);
            }
            catch (Exception e)
            {
                if (HandleError(context, e) == ExceptionAction.Rethrow)
                {
                    throw;
                }
            }
        }
        
        protected virtual TearDownAction HandleTearDown()
        {
            return TearDownAction.OnExecuteExit;
        }

        /// <summary>
        /// Implement in derived command.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        protected abstract Task DoExecuteAsync(TContext context);

        /// <summary>
        /// Implement in derived command (optional).
        /// </summary>
        /// <param name="context">The context to execute.</param>
        protected virtual void DoInitialize(TContext context)
        {
        }

        protected virtual void DoTearDown(TContext context)
        {
        }
        
        /// <summary>
        /// Implement in your command to handle any exceptions.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>ExceptionAction that specifies how the exception should be handled.</returns>
        protected abstract ExceptionAction HandleError(TContext context, Exception exception);

        protected virtual void Dispose(bool disposing)
        {            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            if (HandleTearDown() == TearDownAction.OnDispose)
                TearDown(_context);
        }
    }

}
