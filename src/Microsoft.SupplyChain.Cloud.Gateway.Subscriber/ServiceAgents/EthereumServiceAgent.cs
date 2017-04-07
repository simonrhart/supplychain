using System;
using Microsoft.SupplyChain.Services.Contracts;
using Nethereum.Web3;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.ServiceAgents
{
    public class EthereumServiceAgent : IBlockchainServiceAgent
    {
        private bool _disposed;
        private Web3 web3;

        public EthereumServiceAgent(string transactionNodeVip)
        {

        }

        public void Publish<TPayload>(TPayload payload)
        {
            
        }

        public string TransactionNodeVip
        {
            get;set;
        }

       }
}
