using Castle.Windsor;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Configuration
{
    public interface IContainerBuilder
    {
        void BuildCommands();
        IWindsorContainer Container { get; }
        void BuildServiceAgents();

        void Build();
    }
}
