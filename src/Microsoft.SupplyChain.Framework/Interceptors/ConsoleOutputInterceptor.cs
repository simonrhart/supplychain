using System.Diagnostics;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Anglia.Framework.Interceptors
{
    public class ConsoleOutputInterceptor : IInterceptor
    {
        private readonly ILogger _logger;

        public ConsoleOutputInterceptor(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            var sw = Stopwatch.StartNew();
            _logger.DebugFormat("Invoking {0}.{1}", invocation.InvocationTarget.ToString(), invocation.Method.Name);
            // forward the call onto the target.
            try
            {
                invocation.Proceed();
            }
            finally
            {
                _logger.DebugFormat("{0}.{1} completed in {2}ms", invocation.InvocationTarget.ToString(),
                    invocation.Method.Name, sw.ElapsedMilliseconds);
            }
        }
    }
}
