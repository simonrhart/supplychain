using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Microsoft.SupplyChain.Framework.Mvc
{
    public class CastleDependencyResolver : IDependencyResolver
    {
        protected IWindsorContainer _container;

        public CastleDependencyResolver(IWindsorContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType).Cast<object>();
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope()
        {
            IWindsorContainer childContainer = new WindsorContainer();
            _container.AddChildContainer(childContainer);
            return new CastleDependencyResolver(childContainer);
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
