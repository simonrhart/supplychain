using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperContext : BaseContext
    {
        public BlockchainContractBootstrapperContext(StatelessService serviceInstance, string transactionNodeVip, string source)
        {
            StatelessServiceInstance = serviceInstance;
            TransactionNodeVip = transactionNodeVip;
            Source = source;
        }

        public StatelessService StatelessServiceInstance { get; }   
        
        public string Source { get; }

        public string TransactionNodeVip { get; set; }
    }
}
