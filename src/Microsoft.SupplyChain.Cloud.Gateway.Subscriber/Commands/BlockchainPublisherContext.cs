using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherContext : BaseContext
    {
        public BlockchainPublisherContext(StatelessService serviceInstance, string transactionNodeVip)
        {
            FabricServiceInstance = serviceInstance;
            TransactionNodeVip = transactionNodeVip;
        }

        public StatelessService FabricServiceInstance { get; }       

        public string TransactionNodeVip { get; set; }
    }
}
