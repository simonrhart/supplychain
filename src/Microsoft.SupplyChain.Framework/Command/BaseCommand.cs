using System;

namespace Microsoft.SupplyChain.Framework
{
    /// <summary>
    /// Base command 
    /// </summary>
    /// <typeparam name="TContext">The context for this command.</typeparam>
    public abstract class BaseCommand<TContext> : ICommand<TContext> where TContext : BaseContext
    {
        
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

        /// <summary>
        /// Executes the passed context.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        public void Execute(TContext context)
        {
            try
            {
                Initialize(context);
                DoExecute(context);
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
                TearDown(context);
            }
        }

        internal void Initialize(TContext context)
        {
            try
            {
                DoInitialize(context);
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
        
        /// <summary>
        /// Implement in derived command.
        /// </summary>
        /// <param name="context">The context to execute.</param>
        protected abstract void DoExecute(TContext context);

        /// <summary>
        /// Implement in derived command (optional).
        /// </summary>
        /// <param name="context">The context to execute.</param>
        protected virtual void DoInitialize(TContext context)
        {
        }

        protected virtual void DoTearDown(TContext context)
        {}
        
        /// <summary>
        /// Implement in your command to handle any exceptions.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="exception">The exception thrown.</param>
        /// <returns>ExceptionAction that specifies how the exception should be handled.</returns>
        protected abstract ExceptionAction HandleError(TContext context, Exception exception);
    }

}
