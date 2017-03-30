using Castle.Windsor;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Configuration
{
    public interface IContainerBuilder
    {
        void BuildCommands();
        IWindsorContainer Container { get; }
        void BuildServiceAgents();

        IServiceLocator Build();
    }
}
