using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherContext : BaseContext
    {
        public BlockchainPublisherContext(StatelessService serviceInstance, string transactionNodeVip)
        {
            StatelessServiceInstance = serviceInstance;
            TransactionNodeVip = transactionNodeVip;
        }

        public StatelessService StatelessServiceInstance { get; }

        public string TransactionNodeVip { get; set; }
    }
}