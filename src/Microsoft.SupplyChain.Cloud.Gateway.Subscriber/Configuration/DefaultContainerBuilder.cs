using Castle.Windsor;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands;
using Microsoft.SupplyChain.Framework;
using Castle.MicroKernel.Registration;
using Castle.Core;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Configuration
{
    public class DefaultContainerBuilder : IContainerBuilder
    {
        public IWindsorContainer _container;

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

        public void Build()
        {

        }
    }
}
