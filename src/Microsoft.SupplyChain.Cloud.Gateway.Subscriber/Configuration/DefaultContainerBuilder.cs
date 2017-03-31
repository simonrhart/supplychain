using Castle.Windsor;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands;
using Microsoft.SupplyChain.Framework;
using Castle.MicroKernel.Registration;
using Castle.Core;
using System;
using Castle.DynamicProxy;
using Microsoft.SupplyChain.Framework.Interceptors;
using Microsoft.ServiceBus.Messaging;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Processors;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Configuration
{
    public class DefaultContainerBuilder : IContainerBuilder
    {
        private IWindsorContainer _container;
        private bool _disposed = false;
        private WindsorServiceLocator _windsorServiceLocator;

        public DefaultContainerBuilder()
        {
            _container = new WindsorContainer();           
        }

        public IWindsorContainer Container
        {
            get
            {
                return _container;
            }
        }

        public virtual void BuildCommands()
        {
            _container.Register(Component.For<ICommandAbstractFactory>()
                        .ImplementedBy<CommandAbstractFactory>()
                        .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                        .LifestyleSingleton());         

            _container.Register(Component.For<ICommand<IoTHubSubscriberContext>>()
                .ImplementedBy<IoTHubSubscriberCommand>()
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleTransient());

        }

        public void BuildServiceAgents()
        {
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

        private void BuildProcessors()
        {
            _container.Register(Component.For<IEventProcessor>()
                     .ImplementedBy<GenericEventProcessor>()
                     .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                     .LifestyleSingleton());
        }

        private void BuildServiceFabricServices()
        {
        }

        public IServiceLocator Build()
        {
            BuildAndRegisterServiceLocator();
            BuildInterceptors();
            BuildCommands();
            BuildServiceAgents();
            BuildProcessors();
            BuildServiceFabricServices();
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
