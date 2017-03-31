using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperContext : BaseContext
    {
        public BlockchainContractBootstrapperContext(StatelessService serviceInstance, string transactionNodeVip)
        {
            StatelessServiceInstance = serviceInstance;
            TransactionNodeVip = transactionNodeVip;
        }

        public StatelessService StatelessServiceInstance { get; }       

        public string TransactionNodeVip { get; set; }
    }
}
