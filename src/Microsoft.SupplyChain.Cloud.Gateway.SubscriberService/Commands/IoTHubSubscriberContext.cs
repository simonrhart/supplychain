using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Commands
{
    public class IoTHubSubscriberContext : BaseContext
    {
        public ISubscriberService StatelessInstance { get; }

        public string IoTHubConnectionString { get; set; }

        public string IoTHubStorageConnectionString { get; set; }

        public string IoTHubDeviceToCloudName { get; set; }

        public string IoTHubConsumerGroupName { get; set; }
    }
}
