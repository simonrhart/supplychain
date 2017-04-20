using Castle.Windsor;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Configuration
{
    public interface IContainerBuilder
    {
        void BuildCommands();
        IWindsorContainer Container { get; }
        void BuildServiceAgents();

        IServiceLocator Build();
    }
}
