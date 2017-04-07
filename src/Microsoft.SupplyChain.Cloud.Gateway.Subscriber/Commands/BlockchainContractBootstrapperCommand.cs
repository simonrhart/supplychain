using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Services.Contracts;
using Nethereum.Web3;
using System;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperCommand : BaseCommand<BlockchainContractBootstrapperContext>
    {
        private IBlockchainServiceAgent _blockchainServiceAgent;

        public BlockchainContractBootstrapperCommand(IBlockchainServiceAgent blockchainServiceAgent)
        {
            _blockchainServiceAgent = blockchainServiceAgent;          
        }

        protected override void DoExecute(BlockchainContractBootstrapperContext context)
        {
             // now get whether the smart contract is the latest one deployed or not.
        }

        protected override void DoInitialize(BlockchainContractBootstrapperContext context)
        {            
           
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainContractBootstrapperContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
