using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Commands;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Controllers;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Command;
using Microsoft.SupplyChain.Framework.Interceptors;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration
{
    public class DefaultContainerBuilder : IContainerBuilder
    {
        private readonly IWindsorContainer _container;
        private bool _disposed = false;
        private WindsorServiceLocator _windsorServiceLocator;

        public DefaultContainerBuilder()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IWindsorContainer>().Instance(_container));
            
        }

        public IWindsorContainer Container => _container;

        public virtual void BuildCommands()
        {
            _container.Register(Component.For<ICommandAbstractFactory>()
                .ImplementedBy<CommandAbstractFactory>()
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleSingleton());

            _container.Register(Component.For<ICommand<GenerateIoTSecurityTokenContext>>()
                .ImplementedBy<GenerateIoTSecurityTokenCommand>()
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleTransient());

        }


        private void BuildInterceptors()
        {
            _container.Register(Component.For<IInterceptor>()
                .ImplementedBy<ConsoleOutputInterceptor>()
                .Named("ConsoleInterceptor"));

        }

        private void BuildAndRegisterServiceLocator()
        {
            _windsorServiceLocator = new WindsorServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => _windsorServiceLocator);

            // now register the service locator with castle..
            _container.Register(Component.For<IServiceLocator>().Instance(_windsorServiceLocator));
        }

      

        public virtual void BuildRepositories()
        {
            _container.Register(Component.For<IDeviceStoreRepository>()
                .ImplementedBy<DeviceStoreRepository>()
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleTransient());

        }

        private void BuildControllers()
        {
            _container.Register(Component.For<DiscoveryController>()
                .ImplementedBy<DiscoveryController>()
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleTransient());
        }



        public IServiceLocator Build()
        {
            BuildAndRegisterServiceLocator();
            BuildInterceptors();
            BuildCommands();
            BuildControllers();
            BuildRepositories();
            return _windsorServiceLocator;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _container.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
