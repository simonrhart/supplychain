using Microsoft.ServiceBus.Messaging;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Processors
{
    public class GenericEventProcessorFactory : IEventProcessorFactory
    {
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return ServiceLocator.Current.GetInstance<IEventProcessor>();
        }
    }
}
