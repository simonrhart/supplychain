using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Commands
{
    public class BlockchainPublisherContext : BaseContext
    {
        public BlockchainPublisherContext(string transactionNodeVip)
        {
            TransactionNodeVip = transactionNodeVip;
        }

        public string TransactionNodeVip { get; set; }
    }
}