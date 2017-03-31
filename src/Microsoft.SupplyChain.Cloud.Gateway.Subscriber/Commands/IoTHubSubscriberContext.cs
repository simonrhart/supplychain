using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class IoTHubSubscriberContext : BaseContext
    {
        public IoTHubSubscriberContext(StatelessService serviceInstance)
        {
            StatelessInstance = serviceInstance;
        }

        public StatelessService StatelessInstance { get; }

        public string IoTHubConnectionString { get; set; }

        public string IoTHubStorageConnectionString { get; set; }

        public string IoTHubDeviceToCloudName { get; set; }

        public string IoTHubConsumerGroupName { get; set; }
    }
}
