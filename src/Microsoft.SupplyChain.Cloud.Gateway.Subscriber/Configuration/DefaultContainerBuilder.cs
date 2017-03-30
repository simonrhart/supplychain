using Castle.Windsor;

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

        public void BuildCommands()
        {
        }

        public void BuildServiceAgents()
        {
        }

        public void Build()
        {

        }
    }
}
