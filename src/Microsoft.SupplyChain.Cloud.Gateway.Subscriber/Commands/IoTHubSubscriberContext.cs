using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class IoTHubSubscriberContext : BaseContext
    {
        public ISubscriber StatelessInstance { get; }

        public string IoTHubConnectionString { get; set; }

        public string IoTHubStorageConnectionString { get; set; }

        public string IoTHubDeviceToCloudName { get; set; }

        public string IoTHubConsumerGroupName { get; set; }
    }
}
