using Anglia.Framework.ServiceLocation;
using Microsoft.ServiceBus.Messaging;

namespace Anglia.Cloud.Gateway.Subscriber.Processors
{
    public class GenericEventProcessorFactory : IEventProcessorFactory
    {
        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return ServiceLocator.Current.GetInstance<IEventProcessor>();
        }
    }
}
