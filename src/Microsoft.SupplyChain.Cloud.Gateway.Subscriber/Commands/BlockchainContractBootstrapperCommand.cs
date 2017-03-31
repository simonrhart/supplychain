using Microsoft.SupplyChain.Framework;
using Nethereum.Web3;
using System;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperCommand : BaseCommand<BlockchainContractBootstrapperContext>
    {
        private Web3 web3;

        public BlockchainContractBootstrapperCommand()
        {
            
        }

        protected override void DoExecute(BlockchainContractBootstrapperContext context)
        {
             // now get whether the    
        }

        protected override void DoInitialize(BlockchainContractBootstrapperContext context)
        {            
            web3 = new Web3(context.TransactionNodeVip);
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainContractBootstrapperContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
