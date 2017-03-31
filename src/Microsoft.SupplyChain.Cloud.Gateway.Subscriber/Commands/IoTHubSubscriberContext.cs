using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class IoTHubSubscriberContext : BaseContext
    {
        public IoTHubSubscriberContext(StatelessService serviceInstance)
        {
            FabricServiceInstance = serviceInstance;
        }

        public StatelessService FabricServiceInstance { get; }

        public string IoTHubConnectionString { get; set; }

        public string IoTHubStorageConnectionString { get; set; }

        public string IoTHubDeviceToCloudName { get; set; }
    }
}
